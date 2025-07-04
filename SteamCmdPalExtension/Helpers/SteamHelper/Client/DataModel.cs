using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Linq;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.SteamHelper.Client;

internal partial class Client
{
    private FrozenDictionary<int, string>? _tagsMapping;
    private readonly ConcurrentDictionary<long, EnrichedAppData> _appData = new();
    private readonly ConcurrentDictionary<long, int> _appDataHash = new();
    private readonly ConcurrentDictionary<long, double> _appWeights = new();
    private readonly ConcurrentDictionary<string, long> _titleAppidMapping = new();
    private async Task InitializeDataModel()
    {
        _tagsMapping = (await GetTagsMappingAsync().ConfigureAwait(false)).ToFrozenDictionary();
        await UpdateAsync().ConfigureAwait(false);
        _updateTask = Task.Run(() => UpdateTask());
    }
    internal async Task UpdateAsync()
    {
        await UpdateAppDataAsync().ConfigureAwait(false);
    }
    private async Task UpdateTask()
    {
        await Task.Delay(_interval, _cts.Token).ConfigureAwait(false);
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                await UpdateAsync().ConfigureAwait(false);
                await Task.Delay(_interval, _cts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                ExtensionHost.LogMessage("[SteamCmdPalExtension] Error during data model update: " + ex.Message);
            }
        }
    }

    private EnrichedAppData CreateEnrichedAppData(AppData appData)
    {
        return new EnrichedAppData
        {
            AppId = appData.AppId,
            DisplayName = appData.DisplayName,
            DisplayStatus = appData.DisplayStatus,
            Type = appData.AppType,
            IconHash = appData.IconHash,
            HeaderFilename = appData.HeaderFilename,
            CapsuleFilename = appData.LibraryCapsuleFilename,
            StoreTags = appData.StoreTags?
                .Where(id => _tagsMapping != null && _tagsMapping.TryGetValue(id, out _))
                .Select(id => _tagsMapping![id])
                .ToArray(),
            PlayTime = appData.MinutesPlaytimeForever / 60.0,
            PlayTimeTwoWeeks = appData.MinutesPlaytimeLastTwoWeeks / 60.0,
            LastPlayTime = appData.RtLastTimePlayed,
            LastPlayTimeOrInstalled = appData.RtLastTimePlayedOrInstalled,
            SizeOnDisk = appData.SizeOnDisk,
            SortAs = appData.SortAs,
            VisibleInGameList = appData.VisibleInGameList,
            Installed = appData.Installed,
        };
    }

    private async Task UpdateAppDataAsync()
    {
        long minimumLastTimePlayedOrInstalled = await GetMinimumLastTimePlayedOrInstalledAsync()
            .ConfigureAwait(false);
        foreach ((long appid, AppData appData) in await GetAppDataAsync().ConfigureAwait(false))
        {
            if (!_appData.TryGetValue(appid, out _) ||
                _appDataHash[appid] != appData.GetHashCode())
            {
                _appData[appid] = CreateEnrichedAppData(appData);
                _titleAppidMapping[appData.DisplayName] = appid;
                _appDataHash[appid] = appData.GetHashCode();


                // Weights calculation
                double playtimeForeverWeight = appData.MinutesPlaytimeForever == 0 ? 0 :
                    Math.Pow(Math.Log10(appData.MinutesPlaytimeForever ?? 0), 2) * 0.3334;
                double playtimeLastTwoWeeksWeight =
                    appData.MinutesPlaytimeLastTwoWeeks > 45 ? 45 :
                    -0.074457 * (appData.MinutesPlaytimeLastTwoWeeks ?? 0) +
                    0.034782 * Math.Pow(appData.MinutesPlaytimeLastTwoWeeks ?? 0, 2) -
                    0.002141 * Math.Pow(appData.MinutesPlaytimeLastTwoWeeks ?? 0, 3) +
                    0.000053 * Math.Pow(appData.MinutesPlaytimeLastTwoWeeks ?? 0, 4) -
                    0.0000004 * Math.Pow(appData.MinutesPlaytimeLastTwoWeeks ?? 0, 5);
                _appWeights[appid] = ((
                    (playtimeForeverWeight > 10 ? 10 : playtimeForeverWeight) +
                    (playtimeLastTwoWeeksWeight > 15 ? 15 : playtimeLastTwoWeeksWeight)) / 25 / 2) +
                    ((appData.RtLastTimePlayedOrInstalled == null ||
                    appData.RtLastTimePlayedOrInstalled == 0 ? 0 :
                    (double)appData.RtLastTimePlayedOrInstalled -
                    minimumLastTimePlayedOrInstalled) / (
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds() - minimumLastTimePlayedOrInstalled) / 2);
            }
        }
    }
    internal EnrichedAppData[]? GetSortedAppDataArray()
    {
        return [.. _appWeights
            .Where(pair => _appData.ContainsKey(pair.Key))
            .OrderByDescending(pair => pair.Value)
            .Select(pair =>
            {
                return _appData[pair.Key];
            })
            .Where(line => line != null)];
    }
}
