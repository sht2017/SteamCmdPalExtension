using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-RemoteObject
internal sealed class RemoteObject
{
    internal enum RemoteObjectTypeValue
    {
        @object, function, undefined, @string, number, boolean, symbol, bigint
    }

    internal enum RemoteObjectSubtypeValue
    {
        array, @null, node, regexp, date, map, set, weakmap, weakset,
        iterator, generator, error, proxy, promise, typedarray, arraybuffer,
        dataview, webassemblymemory, wasmvalue
    }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<RemoteObjectTypeValue>))]
    public required RemoteObjectTypeValue Type { get; set; }

    [JsonPropertyName("subtype")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(JsonStringEnumConverter<RemoteObjectSubtypeValue>))]
    public RemoteObjectSubtypeValue? Subtype { get; set; }

    [JsonPropertyName("className")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClassName { get; set; }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? Value { get; set; }

    [JsonPropertyName("unserializableValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UnserializableValue { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("deepSerializedValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeepSerializedValue? DeepSerializedValue { get; set; }

    [JsonPropertyName("objectId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ObjectId { get; set; }

    [JsonPropertyName("preview")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ObjectPreview? Preview { get; set; }

    [JsonPropertyName("customPreview")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CustomPreview? CustomPreview { get; set; }
}