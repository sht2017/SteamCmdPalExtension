using SteamCmdPalExtension.Helpers.WebSocketHelper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper;

internal partial class Debugger
{
    internal static async Task<Debugger> CreateWsAsync(
        Uri targetUrl,
        CancellationToken cancellationToken = default)
    {
        WebSocketHelper.Version version = await WebSocketHelper.Version.GetVersion(targetUrl).ConfigureAwait(false);
        Debugger instance = new(
            await WebSocket.CreateAsync(new Uri(version.WebSocketDebuggerUrl), cancellationToken).ConfigureAwait(false));
        return instance;
    }
}
