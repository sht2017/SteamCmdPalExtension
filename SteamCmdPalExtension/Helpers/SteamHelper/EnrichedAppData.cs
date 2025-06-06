namespace SteamCmdPalExtension.Helpers.SteamHelper;

internal sealed class EnrichedAppData
{
    internal required int AppId { get; set; }
    internal required string DisplayName { get; set; }
    internal required int DisplayStatus { get; set; }
    internal required int Type { get; set; }
    internal string? IconHash { get; set; }
    internal string? HeaderFilename { get; set; }
    internal string? CapsuleFilename { get; set; }
    internal string[]? StoreTags { get; set; }
    internal double? PlayTime { get; set; }
    internal double? PlayTimeTwoWeeks { get; set; }
    internal long? LastPlayTime { get; set; }
    internal long? LastPlayTimeOrInstalled { get; set; }
    internal long? SizeOnDisk { get; set; }
    internal string? SortAs { get; set; }
    internal bool VisibleInGameList { get; set; } = true;
    internal bool Installed { get; set; }
}