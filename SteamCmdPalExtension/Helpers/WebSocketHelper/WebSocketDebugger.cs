using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Helpers.DebuggerHelper.Protocol;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.WebSocketHelper;

internal sealed partial class WebSocket : IProtocol, IAsyncDisposable
{
    private const int MAX_SEND_CHANNEL_SIZE = 50;
    private bool _disposed;
    private Task? _readingLoop;
    private Task? _writingLoop;
    private CancellationTokenSource? _cts;
    private readonly ClientWebSocket _client = new();
    private int sendMessageCount;
    private readonly Channel<string> _sendChannel = Channel.CreateBounded<string>(new BoundedChannelOptions(MAX_SEND_CHANNEL_SIZE)
    {
        SingleReader = true,
        SingleWriter = false,
        FullMode = BoundedChannelFullMode.Wait
    });
    private readonly ConcurrentQueue<string> _receiveMessageQueue = new();
    internal static async Task<WebSocket> CreateAsync(
        Uri webSocketDebuggerUrl,
        CancellationToken cancellationToken = default)
    {
        WebSocket instance = new();
        await instance._client.ConnectAsync(webSocketDebuggerUrl, cancellationToken)
            .ConfigureAwait(false);
        instance.StartProcessing();
        return instance;
    }

    public async Task SendAsync(string message, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        await _sendChannel.Writer.WriteAsync(message, cancellationToken).ConfigureAwait(false);
        Interlocked.Increment(ref sendMessageCount);
    }

    public async Task<string?> ReceiveAsync(
        int timeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        using CancellationTokenSource timeoutCts = new(TimeSpan.FromSeconds(timeoutSeconds));
        using CancellationTokenSource linkedCts = CancellationTokenSource
            .CreateLinkedTokenSource(
            cancellationToken,
            timeoutCts.Token);

        while (true)
        {
            if (_receiveMessageQueue.TryDequeue(out string? message))
            {
                return message;
            }
            await Task.Delay(10, linkedCts.Token).ConfigureAwait(false);
        }
    }

    private void StartProcessing()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_readingLoop != null || _writingLoop != null)
            throw new InvalidOperationException("Processing task is already running.");
        _cts = new CancellationTokenSource();
        _writingLoop = Task.Run(() => WriteLoopAsync());
        _readingLoop = Task.Run(() => ReadLoopAsync());
    }
    private async Task StopProcessingAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_cts == null)
            return;

        _sendChannel.Writer.Complete();
        await _cts.CancelAsync().ConfigureAwait(false);
        try
        {
            await Task.WhenAll(
                _writingLoop ?? Task.CompletedTask,
                _readingLoop ?? Task.CompletedTask).ConfigureAwait(false);
            _writingLoop = null;
            _readingLoop = null;
        }
        catch (Exception) { }
        _cts.Dispose();
        _cts = null;
    }

    private async Task WriteAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        string message = await _sendChannel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        await _client.SendAsync(
            Encoding.UTF8.GetBytes(message),
            WebSocketMessageType.Text,
            true, _cts!.Token).ConfigureAwait(false);
        Interlocked.Decrement(ref sendMessageCount);
    }
    private async Task WriteLoopAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            while (!_cts!.Token.IsCancellationRequested)
            {
                await WriteAsync(_cts.Token).ConfigureAwait(false);
                if (sendMessageCount < MAX_SEND_CHANNEL_SIZE * 3 / 4)
                    await Task.Yield();
            }
        }
        catch (OperationCanceledException) { }
    }

    private async Task ReadLoopAsync(int bufferSize = 4096)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        using MemoryStream bufferStream = new();
        byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        try
        {
            while (!_cts!.Token.IsCancellationRequested)
            {
                WebSocketReceiveResult result = await _client.ReceiveAsync(buffer, _cts!.Token)
                    .ConfigureAwait(false);
                await bufferStream.WriteAsync(buffer.AsMemory(0, result.Count), _cts.Token)
                    .ConfigureAwait(false);

                if (result.EndOfMessage)
                {
                    _receiveMessageQueue.Enqueue(
                        Encoding.UTF8.GetString(
                            bufferStream.GetBuffer().
                            AsSpan(0, (int)bufferStream.Length)));
                    bufferStream.SetLength(0);
                    await Task.Delay(10, _cts.Token).ConfigureAwait(false);
                }
                await Task.Yield();
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public void Dispose()
    {
        if (_disposed) return;
        _client.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }


    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        await StopProcessingAsync().ConfigureAwait(false);
        try
        {
            await _client.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                null,
                CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ExtensionHost.LogMessage($"Error closing WebSocketClient, might not a problem: '{ex.Message}'.");
        }
        Dispose();
    }
}