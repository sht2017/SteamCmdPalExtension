using SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Method;

[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(List<TargetInfo>))]
[JsonSerializable(typeof(GetTargets.Request))]
internal sealed partial class GetTargetsContext : JsonSerializerContext { }

internal static class GetTargets
{
    internal sealed class Request : CDP.Method.Request<Parameters>
    {
        internal Request() : base("Target.getTargets") { }
        protected override JsonTypeInfo CurrentTypeInfo => GetTargetsContext.Default.Request;
    }
    internal sealed class Parameters
    {
        [JsonPropertyName("filter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TargetFilter? TargetFilter { get; set; }
    }
    internal static List<TargetInfo>? Response(JsonRPC.Response response)
    {
        JsonRPC.SuccessResponse successResponse = response.GetSuccess();
        return JsonSerializer.Deserialize(
            successResponse.Result.GetProperty("targetInfos").GetRawText(),
            GetTargetsContext.Default.ListTargetInfo);
    }
}