using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Runtime/#type-SerializationOptions
internal sealed class SerializationOptions
{
    internal enum SerializationMode
    {
        deep,
        json,
        idOnly
    }

    [JsonPropertyName("serialization")]
    [JsonConverter(typeof(JsonStringEnumConverter<SerializationMode>))]
    public required SerializationMode Serialization { get; set; }

    [JsonPropertyName("maxDepth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxDepth { get; set; }

    [JsonPropertyName("additionalParameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? AdditionalParameters { get; set; }

    internal void SetDOMSerializationParameters(int? maxNodeDepth = null, string? includeShadowTree = null)
    {
        AdditionalParameters ??= [];

        if (maxNodeDepth.HasValue)
        {
            AdditionalParameters["maxNodeDepth"] = maxNodeDepth.Value;
        }

        if (includeShadowTree != null)
        {
            if (includeShadowTree is not "none" and not "open" and not "all")
            {
                throw new ArgumentException("includeShadowTree must be one of: \"none\", \"open\", \"all\"");
            }

            AdditionalParameters["includeShadowTree"] = includeShadowTree;
        }
    }
}