using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Target.Type;

// See https://chromedevtools.github.io/devtools-protocol/tot/Target/#type-TargetFilter

internal sealed class TargetFilterEntry
{
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    [JsonPropertyName("exclude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Exclude { get; set; }
}

internal sealed partial class TargetFilter : List<TargetFilterEntry>
{
    public static TargetFilter CreateDefault()
    {
        return
        [
            new TargetFilterEntry { Type = "browser", Exclude = true },
            new TargetFilterEntry { Type = "tab", Exclude = true },
            new TargetFilterEntry()
        ];
    }

    public static TargetFilter IncludeOnly(string targetType)
    {
        return
        [
            new TargetFilterEntry { Type = targetType },
            new TargetFilterEntry { Exclude = true }
        ];
    }

    public static TargetFilter Exclude(params string[] targetTypes)
    {
        var filter = new TargetFilter();
        foreach (var type in targetTypes)
        {
            filter.Add(new TargetFilterEntry { Type = type, Exclude = true });
        }
        filter.Add(new TargetFilterEntry());
        return filter;
    }
}