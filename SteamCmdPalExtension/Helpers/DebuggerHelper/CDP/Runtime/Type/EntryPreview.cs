using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-EntryPreview
internal sealed class EntryPreview
{
    [JsonPropertyName("key")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ObjectPreview? Key { get; set; }

    [JsonPropertyName("value")]
    public required ObjectPreview Value { get; set; }
}