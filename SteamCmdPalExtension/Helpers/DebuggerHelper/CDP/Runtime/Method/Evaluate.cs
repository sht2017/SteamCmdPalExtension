using SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Method;


[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(int))]

[JsonSerializable(typeof(ExceptionDetails))]
[JsonSerializable(typeof(RemoteObject))]

[JsonSerializable(typeof(Evaluate.Request))]
internal sealed partial class EvaluateContext : JsonSerializerContext { }

internal static class Evaluate
{
    internal sealed class Request : CDP.Method.Request<Parameters>
    {
        protected override CDP.Method.Request<Parameters> Current => this;
        [JsonPropertyName("sessionId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SessionId { get; set; }
        internal Request() : base("Runtime.evaluate") { }
        protected override JsonTypeInfo CurrentTypeInfo => EvaluateContext.Default.Request;
    }
    internal sealed class Parameters
    {
        [JsonPropertyName("expression")]
        public required string Expression { get; set; }

        [JsonPropertyName("objectGroup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ObjectGroup { get; set; }

        [JsonPropertyName("includeCommandLineAPI")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IncludeCommandLineAPI { get; set; }

        [JsonPropertyName("silent")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Silent { get; set; }

        [JsonPropertyName("contextId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NS__SteamCmdPalExtension_Helpers_DebuggerHelper_CDP_Runtime_Type__ExecutionContextId? ContextId { get; set; }

        [JsonPropertyName("returnByValue")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReturnByValue { get; set; }

        [JsonPropertyName("generatePreview")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? GeneratePreview { get; set; }

        [JsonPropertyName("userGesture")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? UserGesture { get; set; }

        [JsonPropertyName("awaitPromise")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AwaitPromise { get; set; }

        [JsonPropertyName("throwOnSideEffect")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ThrowOnSideEffect { get; set; }

        [JsonPropertyName("timeout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NS__SteamCmdPalExtension_Helpers_DebuggerHelper_CDP_Runtime_Type__TimeDelta? Timeout { get; set; }

        [JsonPropertyName("disableBreaks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DisableBreaks { get; set; }

        [JsonPropertyName("replMode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReplMode { get; set; }

        [JsonPropertyName("allowUnsafeEvalBlockedByCSP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowUnsafeEvalBlockedByCSP { get; set; }

        [JsonPropertyName("uniqueContextId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UniqueContextId { get; set; }

        [JsonPropertyName("serializationOptions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SerializationOptions? SerializationOptions { get; set; }
    }
    internal static (RemoteObject, ExceptionDetails?) Response(JsonRPC.Response response)
    {
        JsonRPC.SuccessResponse successResponse = response.GetSuccess();

        RemoteObject result = JsonSerializer.Deserialize(
            successResponse.Result.GetProperty("result").GetRawText(),
            EvaluateContext.Default.RemoteObject)
            ?? throw new JsonException("Failed to deserialize RemoteObject");

        ExceptionDetails? exceptionDetails = null;
        if (successResponse.Result.TryGetProperty("exceptionDetails", out var exceptionElement))
        {
            exceptionDetails = JsonSerializer.Deserialize(
                exceptionElement.GetRawText(),
                EvaluateContext.Default.ExceptionDetails);
        }

        return (result, exceptionDetails);
    }
}