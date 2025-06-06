using SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using mRuntime = SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Method;
using mTarget = SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Method;
using tRuntime = SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;
using tTarget = SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper;


[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(JsonRPC.Response))]

internal sealed partial class DebuggerBaseContext : JsonSerializerContext { }

internal partial class DebuggerBase
{
    internal async Task<List<tTarget.TargetInfo>?> GetDebuggerTargetsAsync(
        tTarget.TargetFilter? targetFilters = null)
    {
        mTarget.GetTargets.Request request = new() { Id = 1 };
        if (targetFilters != null)
            request.Params = new mTarget.GetTargets.Parameters
            {
                TargetFilter = targetFilters
            };

        return mTarget.GetTargets.Response(
            await StrictSendAndReceiveAsync(
                request.Json,
                DebuggerBaseContext.Default.Response).ConfigureAwait(false));
    }
    internal async Task<(string, CDP.Event)> AttachDebuggerAsync(
        string targetId,
        bool flatten = true)
    {
        mTarget.AttachToTarget.Request request = new()
        {
            Id = 1,
            Params = new mTarget.AttachToTarget.Parameters
            {
                TargetId = targetId,
                Flatten = flatten
            }
        };
        string[] messages = await StrictSendAndReceivesAsync<string>(request.Json, null, 2).ConfigureAwait(false);
        return mTarget.AttachToTarget.Response(messages);
    }
    private async Task<(tRuntime.RemoteObject, tRuntime.ExceptionDetails?)> FullEvaluateAsync(
        string sessionId,
        string expression,
        string? objectGroup = null,
        bool? includeCommandLineAPI = null,
        bool? silent = null,
        NS__SteamCmdPalExtension_Helpers_DebuggerHelper_CDP_Runtime_Type__ExecutionContextId? contextId = null,
        bool? returnByValue = null,
        bool? generatePreview = null,
        bool? userGesture = null,
        bool? awaitPromise = null,
        bool? throwOnSideEffect = null,
        NS__SteamCmdPalExtension_Helpers_DebuggerHelper_CDP_Runtime_Type__TimeDelta? timeout = null,
        bool? disableBreaks = null,
        bool? replMode = null,
        bool? allowUnsafeEvalBlockedByCSP = null,
        string? uniqueContextId = null,
        SerializationOptions? serializationOptions = null)
    {
        mRuntime.Evaluate.Request request = new()
        {
            Id = 1,
            SessionId = sessionId
        };
        mRuntime.Evaluate.Parameters param = new() { Expression = expression };
        if (objectGroup != null)
            param.ObjectGroup = objectGroup;
        if (includeCommandLineAPI != null)
            param.IncludeCommandLineAPI = includeCommandLineAPI;
        if (silent != null)
            param.Silent = silent;
        if (contextId != null)
            param.ContextId = contextId;
        if (returnByValue != null)
            param.ReturnByValue = returnByValue;
        if (generatePreview != null)
            param.GeneratePreview = generatePreview;
        if (userGesture != null)
            param.UserGesture = userGesture;
        if (awaitPromise != null)
            param.AwaitPromise = awaitPromise;
        if (throwOnSideEffect != null)
            param.ThrowOnSideEffect = throwOnSideEffect;
        if (timeout != null)
            param.Timeout = timeout;
        if (disableBreaks != null)
            param.DisableBreaks = disableBreaks;
        if (replMode != null)
            param.ReplMode = replMode;
        if (allowUnsafeEvalBlockedByCSP != null)
            param.AllowUnsafeEvalBlockedByCSP = allowUnsafeEvalBlockedByCSP;
        if (uniqueContextId != null)
            param.UniqueContextId = uniqueContextId;
        if (serializationOptions != null)
            param.SerializationOptions = serializationOptions;
        request.Params = param;

        return mRuntime.Evaluate.Response(
                await StrictSendAndReceiveAsync(
                    request.Json,
                    DebuggerBaseContext.Default.Response).ConfigureAwait(false));
    }
    internal async Task<(tRuntime.RemoteObject, tRuntime.ExceptionDetails?)> EvaluateAsync(
        string sessionId,
        string expression)
    {
        return await FullEvaluateAsync(
            sessionId,
            expression,
            returnByValue: true,
            awaitPromise: true).ConfigureAwait(false);
    }
}