using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Target/#type-RemoteLocation

internal sealed class RemoteLocation
{
    [JsonPropertyName("host")]
    public required bool Host { get; set; }

    [JsonPropertyName("port")]
    public required string Port { get; set; }
}