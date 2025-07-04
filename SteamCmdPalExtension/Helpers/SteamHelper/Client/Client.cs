using SteamCmdPalExtension.Helpers.DebuggerHelper.CDP.Runtime.Type;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.SteamHelper.Client;


[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(Dictionary<int, string>))]
[JsonSerializable(typeof(Dictionary<long, AppData>))]
internal sealed partial class ClientContext : JsonSerializerContext { }

internal partial class Client
{
    private async Task<Dictionary<int, string>> GetTagsMappingAsync()
    {
        (RemoteObject remoteObject, _) = await _debugger.EvaluateAsync(
            _sessionId,
            "appStore.m_mapStoreTagLocalization").ConfigureAwait(false);
        return JsonSerializer.Deserialize(
            remoteObject.Value.ToString() ??
            string.Empty, ClientContext.Default.DictionaryInt32String) ??
            throw new InvalidOperationException("Failed to fetch TagsMapping");
    }
    private async Task<Dictionary<long, AppData>> GetAppDataAsync()
    {
        (RemoteObject remoteObject, _) = await _debugger.EvaluateAsync(
            _sessionId,
            @"Object.fromEntries(Array.from(appStore.allApps.entries()).map(([key, item]) => {
                const v = item.value_ || item;
                const k = v.appid || key;
                return [k, {
                    app_type: v.app_type,
                    appid: v.appid,
                    display_name: v.display_name,
                    display_status: v.display_status,
                    header_filename: v.header_filename,
                    icon_hash: v.icon_hash,
                    library_capsule_filename: v.library_capsule_filename,
                    store_tags: Array.isArray(v.store_tag) ? v.store_tag : 
                                (v.m_setStoreTags instanceof Set ? Array.from(v.m_setStoreTags) : undefined),
                    minutes_playtime_forever: v.minutes_playtime_forever,
                    minutes_playtime_last_two_weeks: v.minutes_playtime_last_two_weeks,
                    rt_last_time_played: v.rt_last_time_played,
                    rt_last_time_played_or_installed: v.rt_last_time_played_or_installed,
                    size_on_disk: v.size_on_disk ? +v.size_on_disk || null : null,
                    sort_as: v.sort_as,
                    visible_in_game_list: v.visible_in_game_list,
                    installed: v.installed === undefined ? false : v.installed
                }];
            }))").ConfigureAwait(false);
        return JsonSerializer.Deserialize(
            remoteObject.Value.ToString() ??
            string.Empty, ClientContext.Default.DictionaryInt64AppData) ??
            throw new InvalidOperationException("Failed to fetch AppData");
    }
    private async Task<bool> GetAppStoreStatusAsync()
    {
        (RemoteObject remoteObject, _) = await _debugger.EvaluateAsync(
            _sessionId, "appStore.m_bIsInitialized").ConfigureAwait(false);
        return remoteObject.Value?.GetBoolean() ?? false;
    }
    private async Task<long> GetMinimumLastTimePlayedOrInstalledAsync()
    {
        (RemoteObject remoteObject, _) = await _debugger.EvaluateAsync(
            _sessionId,
            @"(() => {
                const validTimes = Array.from(appStore.m_mapApps.data_.entries())
                    .map(([_, item]) => {
                        const v = item.value_ || item;
                        return v.rt_last_time_played_or_installed;
                    })
                    .filter(time => time !== undefined && time !== null && time > 0);
        
                return validTimes.length > 0 ? Math.min(...validTimes) : null;
            })()").ConfigureAwait(false);
        return remoteObject.Value?.GetInt64() ??
            throw new InvalidOperationException("Failed to fetch MinimumLastTimePlayedOrInstalled");
    }

    internal static string GetIconPath(long appid, string? iconHash)
    {
        string path = Path.Combine(
            STEAM_PATH, "appcache/librarycache",
            $"{appid}/{iconHash}.jpg");
        if (Path.Exists(path))
        {
            return path;
        }
        return "Assets/AppIconPlaceholder.png";
    }
    internal static string GetImagePath(long appid, string? filename)
    {
        filename ??= "header.jpg";

        return (new Uri(Path.Combine(
            STEAM_PATH, "appcache/librarycache",
            $"{appid}/{filename}")))
            .AbsoluteUri.Replace("(", "%28").Replace(")", "%29");
    }
}
