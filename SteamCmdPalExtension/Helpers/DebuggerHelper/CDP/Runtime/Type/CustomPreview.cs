using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-CustomPreview
internal sealed class CustomPreview
{
    [JsonPropertyName("header")]
    public required string Header { get; set; }

    [JsonPropertyName("bodyGetterId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BodyGetterId { get; set; }
}