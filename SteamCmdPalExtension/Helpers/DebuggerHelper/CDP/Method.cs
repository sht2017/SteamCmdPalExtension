using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP;

internal static class Method
{
    internal abstract class Request<T> : JsonRPC.Request<T>
    {
        protected virtual Request<T> Current => this;
        protected abstract JsonTypeInfo CurrentTypeInfo { get; }
        protected Request(string method)
        {
            Method = method;
        }
        internal string ToJsonString()
        {
            return JsonSerializer.Serialize(Current, CurrentTypeInfo);
        }
        internal string Json => ToJsonString();
    }
}
