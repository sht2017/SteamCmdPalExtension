using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.JsonRPC;

internal class Base
{
    // Can only be int or string
    [JsonPropertyName("id")]
    [JsonConverter(typeof(BaseConverter))]
    public object? Id { get; set; }
}

internal sealed class BaseConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                if (reader.TryGetInt32(out int intValue))
                {
                    return intValue;
                }
                throw new JsonException("JSON number for ID could not be parsed as a supported integer type.");
            case JsonTokenType.String:
                return reader.GetString();
            case JsonTokenType.Null:
                return null;
            default:
                throw new JsonException($"Unexpected token type {reader.TokenType} when parsing ID. Expected Number, String, or Null.");
        }
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else if (value is int intValue)
        {
            writer.WriteNumberValue(intValue);
        }
        else if (value is string stringValue)
        {
            writer.WriteStringValue(stringValue);
        }
        else
        {
            throw new JsonException($"Cannot serialize ID of type {value.GetType()}. Only int, string, or null are supported.");
        }
    }
}