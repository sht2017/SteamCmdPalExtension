using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-PropertyPreview
internal sealed class PropertyPreview
{
    internal enum PropertyPreviewTypeValue
    {
        @object, function, undefined, @string, number, boolean, symbol, accessor, bigint
    }

    internal enum PropertyPreviewSubtypeValue
    {
        array, @null, node, regexp, date, map, set, weakmap, weakset,
        iterator, generator, error, proxy, promise, typedarray, arraybuffer,
        dataview, webassemblymemory, wasmvalue
    }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<PropertyPreviewTypeValue>))]
    public required PropertyPreviewTypeValue Type { get; set; }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Value { get; set; }

    [JsonPropertyName("valuePreview")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ObjectPreview? ValuePreview { get; set; }

    [JsonPropertyName("subtype")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(JsonStringEnumConverter<PropertyPreviewSubtypeValue>))]
    public PropertyPreviewSubtypeValue? Subtype { get; set; }
}