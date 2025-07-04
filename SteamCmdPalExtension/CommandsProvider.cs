using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Helpers.SteamHelper.Client;
using SteamCmdPalExtension.Pages;
using SteamCmdPalExtension.Properties.Resources;
using System;
using System.Threading.Tasks;

namespace SteamCmdPalExtension;

public partial class CommandsProvider : CommandProvider, IDisposable
{
    private ICommandItem[] _commands;
    private Client? _client;
    private static readonly Settings _settings = new();

    public CommandsProvider()
    {
        Id = "SteamPal";
        DisplayName = Resources.displayName;
        Icon = IconHelpers.FromRelativePaths("Assets/StoreLogo-Transparent.png", "Assets/StoreLogo-Dark.png");
        Settings = _settings.Settings;
        Frozen = false;

        _commands = [
            new CommandItem(){
                Title = DisplayName,
                Subtitle = Resources.initializingSubtitle,
                Icon = IconHelpers.FromRelativePaths("Assets/StoreLogo-Transparent.png", "Assets/StoreLogo-Dark.png")
            }
        ];
    }

    public override ICommandItem[] TopLevelCommands() => _commands;

    public override void InitializeWithHost(IExtensionHost host)
    {
        base.InitializeWithHost(host);
        _ = Task.Run(InitializeClientAsync);
    }

    private async Task InitializeClientAsync()
    {
        try
        {
            ExtensionHost.LogMessage("[SteamCmdPalExtension.CommandsProvider] Start initializing Steam client");
            _client = await Client.CreateWsAsync(_settings.DebuggerPort).ConfigureAwait(false);
            ExtensionHost.LogMessage("[SteamCmdPalExtension.CommandsProvider] Steam client has been successfully initialized");

            _commands = [
                new CommandItem(new ResultPage(_settings, _client)) {
                    Title = DisplayName,
                    Subtitle = Resources.initializedSubtitle,
                    Icon = IconHelpers.FromRelativePaths("Assets/StoreLogo-Transparent.png", "Assets/StoreLogo-Dark.png")
                },
            ];
            RaiseItemsChanged();
        }
        catch (Exception exception)
        {
            ExtensionHost.LogMessage(
                $"[SteamCmdPalExtension.CommandsProvider] Failed to initialize Steam client, caused by {exception.Message}");
            _commands = [
                new CommandItem(){
                    Title = DisplayName,
                    Subtitle = Resources.initializeFailedSubtitle,
                    Icon = IconHelpers.FromRelativePaths("Assets/StoreLogo-Transparent.png", "Assets/StoreLogo-Dark.png")
                }
            ];
            RaiseItemsChanged();
        }
    }
    public new void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }
}
