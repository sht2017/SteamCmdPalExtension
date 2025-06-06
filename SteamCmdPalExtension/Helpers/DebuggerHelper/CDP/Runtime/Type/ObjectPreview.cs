using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-ObjectPreview
internal sealed class ObjectPreview
{
    internal enum ObjectPreviewTypeValue
    {
        @object, function, undefined, @string, number, boolean, symbol, bigint
    }

    internal enum ObjectPreviewSubtypeValue
    {
        array, @null, node, regexp, date, map, set, weakmap, weakset,
        iterator, generator, error, proxy, promise, typedarray, arraybuffer,
        dataview, webassemblymemory, wasmvalue
    }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<ObjectPreviewTypeValue>))]
    public required ObjectPreviewTypeValue Type { get; set; }

    [JsonPropertyName("subtype")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(JsonStringEnumConverter<ObjectPreviewSubtypeValue>))]
    public ObjectPreviewSubtypeValue? Subtype { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("overflow")]
    public required bool Overflow { get; set; }

    [JsonPropertyName("properties")]
    public required PropertyPreview[] Properties { get; set; }

    [JsonPropertyName("entries")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EntryPreview[]? Entries { get; set; }
}