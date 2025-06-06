using SteamCmdPalExtension.Helpers.DebuggerHelper.Protocol;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper;

internal sealed partial class Debugger : DebuggerBase
{
    private Debugger(IProtocol protocol) : base(protocol) { }
}
