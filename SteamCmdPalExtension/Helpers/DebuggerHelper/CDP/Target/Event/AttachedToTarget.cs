using SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Event;

// See https://chromedevtools.github.io/devtools-protocol/tot/Target/#event-attachedToTarget

internal sealed class AttachedToTarget
{
    [JsonPropertyName("sessionId")]
    public required NS__SteamCmdPalExtension_Helpers_DebuggerHelper_CDP_Target_Type__SessionId SessionId { get; set; }

    [JsonPropertyName("targetInfo")]
    public required TargetInfo TargetInfo { get; set; }

    [JsonPropertyName("waitingForDebugger")]
    public required bool WaitingForDebugger { get; set; }
}
