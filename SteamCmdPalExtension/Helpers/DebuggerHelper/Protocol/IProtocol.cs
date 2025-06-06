using System;
using System.Threading;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.Protocol;

internal interface IProtocol : IDisposable
{
    Task SendAsync(string message, CancellationToken cancellationToken = default);
    Task<string?> ReceiveAsync(int timeoutSeconds = 5, CancellationToken cancellationToken = default);
}
