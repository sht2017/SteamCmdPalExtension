using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-StackTraceId
internal sealed class StackTraceId
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("debuggerId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DebuggerId { get; set; }
}