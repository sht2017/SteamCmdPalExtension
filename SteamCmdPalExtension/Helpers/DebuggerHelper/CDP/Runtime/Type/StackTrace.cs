using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-StackTrace
internal sealed class StackTrace
{
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("callFrames")]
    public required CallFrame[] CallFrames { get; set; }

    [JsonPropertyName("parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StackTrace? Parent { get; set; }

    [JsonPropertyName("parentId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StackTraceId? ParentId { get; set; }
}