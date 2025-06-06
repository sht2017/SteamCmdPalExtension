using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-DeepSerializedValue
internal sealed class DeepSerializedValue
{
    internal enum DeepSerializedValueTypeValue
    {
        undefined, @null, @string,
        number, boolean, bigint,
        regexp, date, symbol,
        array, @object, function,
        map, set, weakmap,
        weakset, error, proxy,
        promise, typedarray, arraybuffer,
        node, window, generator
    }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<DeepSerializedValueTypeValue>))]
    public required DeepSerializedValueTypeValue Type { get; set; }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? Value { get; set; }

    [JsonPropertyName("objectId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ObjectId { get; set; }

    [JsonPropertyName("weakLocalObjectReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? WeakLocalObjectReference { get; set; }
}