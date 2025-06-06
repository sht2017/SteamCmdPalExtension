using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Commands;
using SteamCmdPalExtension.Helpers.SteamHelper;
using SteamCmdPalExtension.Helpers.SteamHelper.Client;
using SteamCmdPalExtension.Properties.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.System;

namespace SteamCmdPalExtension.Pages
{
    internal sealed partial class ResultPage : DynamicListPage
    {
        private readonly Settings _settings;
        private readonly Client _client;
        private readonly List<ListItem> _filteredItems = [];

        public ResultPage(Settings settings, Client client)
        {
            _settings = settings;
            _client = client;
            Title = Resources.displayName;
            Icon = IconHelpers.FromRelativePaths("Assets/StoreLogo-Transparent.png", "Assets/StoreLogo-Dark.png");
            PlaceholderText = Resources.queryPlaceholder;
            ShowDetails = true;
            UpdateItems();
        }

        public override IListItem[] GetItems() => [.. _filteredItems];

        public override void UpdateSearchText(string oldSearch, string newSearch)
        {
            UpdateItems(newSearch);
        }
        private static string FormatLastPlayTime(long lastPlayTime)
        {
            DateTimeOffset playTime = DateTimeOffset.FromUnixTimeSeconds(lastPlayTime);
            DateTimeOffset now = DateTimeOffset.Now;
            int days = (now.Date - playTime.Date).Days;
            CultureInfo culture = CultureInfo.CurrentCulture;

            if (days == 0) return Resources.today;
            if (days == 1) return Resources.yesterday;

            string format = playTime.Year == now.Year ?
                culture.DateTimeFormat.MonthDayPattern :
                culture.DateTimeFormat.LongDatePattern;

            return playTime.ToString(format);
        }

        private void UpdateItems(string? query = null)
        {
            _filteredItems.Clear();
            IsLoading = true;
            EnrichedAppData[]? apps = _client.GetSortedAppDataArray();
            if (apps == null) return;

            IEnumerable<EnrichedAppData> filteredGames = string.IsNullOrEmpty(query)
                ? apps
                : apps.Where(g => g.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase));

            foreach (EnrichedAppData app in filteredGames.Take(_settings.MaxResult))
            {
                ExtensionHost.LogMessage($"Appid: '{app.AppId}', HeaderFilename: '{app.HeaderFilename}'");
                List<IDetailsElement> metadata = [
                        new DetailsElement
                        {
                            Key = Resources.type,
                            Data = new DetailsTags
                            {
                                Tags = [new Tag(Client.GetAppType(app.Type))]
                            }
                        }
                    ];
                if (app.StoreTags != null && app.StoreTags.Length > 0)
                {
                    metadata.Add(new DetailsElement
                    {
                        Key = Resources.tags,
                        Data = new DetailsTags
                        {
                            Tags = app.StoreTags?.Take(5).Select(tag => new Tag(tag)).ToArray() ?? [new Tag(Resources.unknown)]
                        }
                    });
                }
                metadata.AddRange([
                        new DetailsElement
                        {
                            Key = Resources.playTime,
                            Data = new DetailsTags
                            {
                                Tags = [new Tag($"{app.PlayTime:F1} {Resources.hours}")]
                            }
                        },
                        new DetailsElement
                        {
                            Key = Resources.playTimeLast2Weeks,
                            Data = new DetailsTags
                            {
                                Tags = [new Tag($"{app.PlayTimeTwoWeeks:F1} {Resources.hours}")]
                            }
                        }
                    ]);


                Details details = new()
                {
                    Body = $"![]({Client.GetImagePath(app.AppId, app.HeaderFilename)})",
                    Metadata = [.. metadata]
                };

                ListItem item = new(new RunAppCommand(app.AppId))
                {
                    Icon = IconHelpers.FromRelativePath(Client.GetIconPath(app.AppId, app.IconHash)),
                    Title = app.DisplayName,
                    Details = details,
                    TextToSuggest = app.DisplayName,
                    MoreCommands = [
                        new CommandContextItem(new AppPropertiesCommand(app.AppId))
                        {
                            RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.Enter, 0)
                        }
                    ]
                };
                if (app.DisplayStatus == 4)
                    item.Tags = [
                        new Tag(Client.GetAppDisplayStatus(app.DisplayStatus))
                        {
                            Foreground = ColorHelpers.FromRgb(46, 204, 113)
                        }
                    ];
                if (app.DisplayStatus == 1)
                    item.Tags = [
                        new Tag(Client.GetAppDisplayStatus(app.DisplayStatus))
                        {
                            Foreground = ColorHelpers.FromRgb(241, 196, 15)
                        }
                    ];
                if (app.LastPlayTime != null)
                {
                    item.Subtitle = $"{Resources.lastPlayed} {FormatLastPlayTime((long)app.LastPlayTime)}";
                }
                _filteredItems.Add(item);
            }
            if (!string.IsNullOrEmpty(query))
            {
                _filteredItems.Add(new ListItem(new InStoreSearchCommand(query))
                {
                    Title = Resources.searchInStoreTitle,
                    Subtitle = string.Format(Resources.searchInStoreSubtitle, query),
                    Icon = IconHelpers.FromRelativePath("Assets/SteamLogo.png")
                });

            }

            RaiseItemsChanged(_filteredItems.Count);
            IsLoading = false;
        }

        public override void LoadMore() => HasMoreItems = false;
    }
}