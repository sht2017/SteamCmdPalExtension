using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Target/#type-TargetInfo

internal sealed class TargetInfo
{
    [JsonPropertyName("targetId")]
    public required string TargetId { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("attached")]
    public required bool Attached { get; set; }

    [JsonPropertyName("openerId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpenerId { get; set; }

    [JsonPropertyName("canAccessOpener")]
    public required bool CanAccessOpener { get; set; }

    [JsonPropertyName("openerFrameId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OpenerFrameId { get; set; }

    [JsonPropertyName("browserContextId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BrowserContextId { get; set; }

    [JsonPropertyName("subtype")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Subtype { get; set; }
}
