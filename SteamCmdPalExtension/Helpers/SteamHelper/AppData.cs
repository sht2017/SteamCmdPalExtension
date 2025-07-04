using System;
using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.SteamHelper;

internal sealed class AppData
{
    [JsonPropertyName("app_type")]
    public required int AppType { get; set; }

    [JsonPropertyName("appid")]
    public required long AppId { get; set; }

    [JsonPropertyName("display_name")]
    public required string DisplayName { get; set; }

    [JsonPropertyName("display_status")]
    public required int DisplayStatus { get; set; }

    [JsonPropertyName("header_filename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HeaderFilename { get; set; }

    [JsonPropertyName("icon_hash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconHash { get; set; }

    [JsonPropertyName("library_capsule_filename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LibraryCapsuleFilename { get; set; }

    [JsonPropertyName("store_tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? StoreTags { get; set; }

    [JsonPropertyName("minutes_playtime_forever")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinutesPlaytimeForever { get; set; }

    [JsonPropertyName("minutes_playtime_last_two_weeks")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinutesPlaytimeLastTwoWeeks { get; set; }

    [JsonPropertyName("rt_last_time_played")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? RtLastTimePlayed { get; set; }

    [JsonPropertyName("rt_last_time_played_or_installed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? RtLastTimePlayedOrInstalled { get; set; }

    [JsonPropertyName("size_on_disk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? SizeOnDisk { get; set; }

    [JsonPropertyName("sort_as")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SortAs { get; set; }

    [JsonPropertyName("visible_in_game_list")]
    public bool VisibleInGameList { get; set; } = true;

    [JsonPropertyName("installed")]
    public bool Installed { get; set; } = false;

    public override int GetHashCode()
    {
        HashCode hashCode = new();

        hashCode.Add(AppType);
        hashCode.Add(AppId);
        hashCode.Add(DisplayName);
        hashCode.Add(DisplayStatus);
        hashCode.Add(HeaderFilename);
        hashCode.Add(IconHash);
        hashCode.Add(LibraryCapsuleFilename);

        if (StoreTags == null || StoreTags.Length == 0)
            hashCode.Add(-1);

        foreach (int tag in StoreTags ?? [])
        {
            hashCode.Add(tag);
        }

        hashCode.Add(MinutesPlaytimeForever);
        hashCode.Add(MinutesPlaytimeLastTwoWeeks);
        hashCode.Add(RtLastTimePlayed);
        hashCode.Add(RtLastTimePlayedOrInstalled);
        hashCode.Add(SizeOnDisk);
        hashCode.Add(SortAs);
        hashCode.Add(VisibleInGameList);
        hashCode.Add(Installed);

        return hashCode.ToHashCode();
    }
}