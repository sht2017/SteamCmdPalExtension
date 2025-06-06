using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.JsonRPC;

[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(SuccessResponse))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(Error<JsonElement>))]
internal sealed partial class ResponseContext : JsonSerializerContext { }

[JsonConverter(typeof(ResponseConverter))]
internal abstract class Response : Base
{
    internal SuccessResponse GetSuccess()
    {
        if (this is SuccessResponse successResponse)
        {
            return successResponse;
        }
        else if (this is ErrorResponse errorResponse)
        {
            throw new InvalidOperationException($"Error: {errorResponse.Error.Message} (Code: {errorResponse.Error.Code})\n\t{errorResponse.Error.Data}");
        }
        throw new InvalidOperationException($"Unexpected response type: neither success nor failure");
    }
}

internal sealed class SuccessResponse : Response
{
    [JsonPropertyName("result")]
    public required JsonElement Result { get; set; }
}

internal sealed class ErrorResponse : Response
{
    [JsonPropertyName("error")]
    public required Error<JsonElement> Error { get; set; }
}

internal sealed class ResponseConverter : JsonConverter<Response>
{
    public override Response? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        JsonElement root = document.RootElement;
        object id;
        JsonElement idElement = root.GetProperty("id");
        if (idElement.ValueKind == JsonValueKind.Number)
            id = idElement.GetInt32();
        else
            id = idElement.GetString() ?? "";

        if (root.TryGetProperty("result", out JsonElement resultElement))
        {
            return new SuccessResponse { Id = id, Result = resultElement.Clone() };
        }
        else if (root.TryGetProperty("error", out JsonElement errorElement))
        {
            int code = errorElement.GetProperty("code").GetInt32();
            string message = errorElement.GetProperty("message").GetString() ?? "";

            Error<JsonElement> error = new()
            {
                Code = code,
                Message = message
            };

            if (errorElement.TryGetProperty("data", out JsonElement dataElement))
            {
                error.Data = dataElement.Clone();
            }

            return new ErrorResponse { Id = id, Error = error };
        }

        throw new JsonException("Illegal response: missing 'result' or 'error' property");
    }

    public override void Write(Utf8JsonWriter writer, Response value, JsonSerializerOptions options)
    {
        if (value is SuccessResponse successResponse)
        {
            JsonSerializer.Serialize(writer, successResponse, ResponseContext.Default.SuccessResponse);
        }
        else if (value is ErrorResponse errorResponse)
        {
            JsonSerializer.Serialize(writer, errorResponse, ResponseContext.Default.ErrorResponse);
        }
        else
        {
            throw new JsonException("Illegal response: unknown response type");
        }
    }
}
