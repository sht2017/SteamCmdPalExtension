using SteamCmdPalExtension.Helpers.DebuggerHelper.Protocol;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper;

internal partial class DebuggerBase : IDisposable
{
    private readonly IProtocol _protocol;

    protected DebuggerBase(IProtocol protocol)
    {
        _protocol = protocol;
    }
    private async Task SendAsync(
        string message,
        CancellationToken cancellationToken = default)
    {
        await _protocol.SendAsync(message, cancellationToken).ConfigureAwait(false);
    }
    private async Task<string?> ReceiveAsync(
        int timeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        return await _protocol.ReceiveAsync(timeoutSeconds, cancellationToken).ConfigureAwait(false);
    }
    private static T? Deserialize<T>(string? message, JsonTypeInfo<T>? TInfo)
    {
        if (message == null)
            return default;
        if (TInfo == null)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)message;
            throw new ArgumentNullException(nameof(TInfo), "TInfo cannot be null if T is not string.");
        }
        return JsonSerializer.Deserialize(message, TInfo);
    }
    private static T StrictDeserialize<T>(string? message, JsonTypeInfo<T>? TInfo)
    {
        ArgumentNullException.ThrowIfNull(message);
        return Deserialize(message, TInfo) ?? throw new InvalidDataException("Empty message");
    }
    private async Task<T?> ReceiveAsync<T>(
        JsonTypeInfo<T>? TInfo,
        int timeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        string? message = await ReceiveAsync(timeoutSeconds, cancellationToken).ConfigureAwait(false);
        return Deserialize(message, TInfo);
    }
    private async Task<T> StrictReceiveAsync<T>(
        JsonTypeInfo<T>? TInfo,
        int timeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        string? message = await ReceiveAsync(timeoutSeconds, cancellationToken).ConfigureAwait(false);
        return StrictDeserialize(message, TInfo);
    }
    private async Task<T?[]> SendAndReceivesAsync<T>(
        string message,
        JsonTypeInfo<T>? TInfo,
        int receiveCount,
        int singleReceiveTimeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        T?[] result = new T?[receiveCount];
        await SendAsync(message, cancellationToken).ConfigureAwait(false);
        for (int i = 0; i < receiveCount; i++)
        {
            result[i] = await ReceiveAsync(TInfo, singleReceiveTimeoutSeconds, cancellationToken).ConfigureAwait(false);
        }
        return result;
    }
    private async Task<T[]> StrictSendAndReceivesAsync<T>(
        string message,
        JsonTypeInfo<T>? TInfo,
        int receiveCount,
        int singleReceiveTimeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        T[] result = new T[receiveCount];
        await SendAsync(message, cancellationToken).ConfigureAwait(false);
        for (int i = 0; i < receiveCount; i++)
        {
            result[i] = await StrictReceiveAsync(TInfo, singleReceiveTimeoutSeconds, cancellationToken).ConfigureAwait(false);
        }
        return result;
    }
    private async Task<T?> SendAndReceiveAsync<T>(
        string message,
        JsonTypeInfo<T>? TInfo,
        int singleReceiveTimeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        return (await SendAndReceivesAsync(
            message,
            TInfo,
            1,
            singleReceiveTimeoutSeconds,
            cancellationToken).ConfigureAwait(false))[0];
    }
    private async Task<T> StrictSendAndReceiveAsync<T>(
        string message,
        JsonTypeInfo<T>? TInfo,
        int singleReceiveTimeoutSeconds = 5,
        CancellationToken cancellationToken = default)
    {
        return (await StrictSendAndReceivesAsync(
            message,
            TInfo,
            1,
            singleReceiveTimeoutSeconds,
            cancellationToken).ConfigureAwait(false))[0];
    }
    public void Dispose()
    {
        _protocol.Dispose();
    }
}