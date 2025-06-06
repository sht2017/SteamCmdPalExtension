using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Helpers.DebuggerHelper;
using SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Process = SteamCmdPalExtension.Helpers.SteamHelper.ProcessManager;

namespace SteamCmdPalExtension.Helpers.SteamHelper.Client;

internal sealed partial class Client : IDisposable
{
    private const string STEAM = "steam.exe";
    private const string STEAM_CEF = "steamwebhelper.exe";
    private const string TARGET = "SharedJSContext";

    private int _interval;
    private bool _disposed;
    private Task? _updateTask;
    private readonly CancellationTokenSource _cts;
    private readonly Debugger _debugger;
    private readonly string _sessionId;
    private Client(Debugger debugger, string sessionId, int interval = 3000)
    {
        _interval = interval;
        _cts = new();
        _debugger = debugger;
        _sessionId = sessionId;
    }
    public void Dispose()
    {
        if (_disposed) return;
        Dispose(true);
        _disposed = true;
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cts.Cancel();
            Task.WaitAll(_updateTask ?? Task.CompletedTask);
            _cts.Dispose();
            _updateTask?.Dispose();
            _updateTask = null;
            _debugger.Dispose();
        }
    }
    private static async Task<Client> CreateAsync(string arguments, Func<Task<Debugger>> CreateDebugger, int initInterval = 1000, int updateInterval = 1000)
    {
        Process.Kill(STEAM);
        Process.Kill(STEAM_CEF);
        Process.Start(
            Path.Combine(STEAM_PATH, STEAM),
            arguments);
        await Process.WaitForAsync(STEAM_CEF).ConfigureAwait(false);
        Debugger debugger = await CreateDebugger().ConfigureAwait(false);
        while (true)
        {
            foreach (TargetInfo? target in await debugger.GetDebuggerTargetsAsync().ConfigureAwait(false) ?? [])
            {
                if (target.Title.Equals(TARGET))
                {
                    (string sessionId, _) = await debugger.AttachDebuggerAsync(target.TargetId).ConfigureAwait(false);
                    Client? client = null;
                    try
                    {
                        client = new(debugger, sessionId, updateInterval);
                        while (!await client.GetAppStoreStatusAsync().ConfigureAwait(false))
                            await Task.Delay(initInterval).ConfigureAwait(false);
                        await client.InitializeAsync().ConfigureAwait(false);

                        Client result = client;
                        client = null;
                        return result;
                    }
                    finally
                    {
                        client?.Dispose();
                    }
                }
                await Task.Delay(initInterval).ConfigureAwait(false);
            }
        }
    }

    internal static async Task<Client> CreateWsAsync(int? port = null, int interval = 1000)
    {
        port ??= AvailablePort;
        //ExtensionHost.LogMessage($"Starting Steam at {port}");
        return await CreateAsync(
            $"-cef-enable-debugging -devtools-port {port}",
            () => Debugger.CreateWsAsync(new Uri($"http://127.0.0.1:{port}")),
            interval).ConfigureAwait(false);
    }

    internal static async Task<Client> CreatePipeAsync()
    {
        await Task.Yield();
        throw new NotImplementedException("Pipe debugger is not implemented yet.");
    }

    private async Task InitializeAsync()
    {
        ExtensionHost.LogMessage("Initializing Client...");
        await InitializeDataModel().ConfigureAwait(false);
    }
}
