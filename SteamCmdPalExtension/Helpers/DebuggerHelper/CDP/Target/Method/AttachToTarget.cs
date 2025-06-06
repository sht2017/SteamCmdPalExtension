using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Method;

[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(JsonRPC.Response))]
[JsonSerializable(typeof(CDP.Event))]
[JsonSerializable(typeof(AttachToTarget.Request))]
internal sealed partial class AttachToTargetContext : JsonSerializerContext { }

internal static class AttachToTarget
{
    internal sealed class Request : CDP.Method.Request<Parameters>
    {
        internal Request() : base("Target.attachToTarget") { }
        protected override JsonTypeInfo CurrentTypeInfo => AttachToTargetContext.Default.Request;
    }
    internal sealed class Parameters
    {
        [JsonPropertyName("targetId")]
        public required NS__SteamCmdPalExtension_Helpers_DebuggerHelper_CDP_Target_Type__TargetId TargetId { get; set; }

        [JsonPropertyName("flatten")]
        public required bool Flatten { get; set; }
    }
    internal static (string, CDP.Event) Response(string[] messages)
    {
        JsonRPC.Response? response;
        CDP.Event? @event;
        try
        {
            response = JsonSerializer.Deserialize(
                messages[0],
                AttachToTargetContext.Default.Response);
            @event = JsonSerializer.Deserialize(
                messages[1],
                AttachToTargetContext.Default.Event);
        }
        catch (KeyNotFoundException)
        {
            response = JsonSerializer.Deserialize(
                messages[1],
                AttachToTargetContext.Default.Response);
            @event = JsonSerializer.Deserialize(
                messages[0],
                AttachToTargetContext.Default.Event);
        }
        if (response == null || @event == null)
        {
            throw new InvalidDataException("Invalid return: expect both response and event");
        }
        return (
            response.GetSuccess().Result.GetProperty("sessionId").ToString(),
            @event);
    }
}