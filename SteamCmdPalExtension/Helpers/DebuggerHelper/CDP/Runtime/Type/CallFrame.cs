using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-CallFrame
internal sealed class CallFrame
{
    [JsonPropertyName("functionName")]
    public required string FunctionName { get; set; }

    [JsonPropertyName("scriptId")]
    public required string ScriptId { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("lineNumber")]
    public required int LineNumber { get; set; }

    [JsonPropertyName("columnNumber")]
    public required int ColumnNumber { get; set; }
}