using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.Win32;
using SteamCmdPalExtension.Properties.Resources;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SteamCmdPalExtension.Helpers.SteamHelper.Client;

internal partial class Client
{
    private static readonly string STEAM_PATH = GetSteamPath();
    private static string GetSteamPath()
    {
        (string, string)[] steamKeys =
        [
            (@"HKEY_CURRENT_USER\Software\Valve\Steam","SteamPath"),
            (@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam","InstallPath")
        ];
        try
        {
            string? steamPath = null;
            foreach ((string key, string value) in steamKeys)
            {
                steamPath = Path.GetFullPath(
                    Registry.GetValue(key, value, null)?.ToString() ??
                    string.Empty);
                if (!string.IsNullOrEmpty(steamPath) && !Path.Exists(steamPath))
                {
                    break;
                }
            }
            return steamPath ?? throw new FileNotFoundException("Cannot find steam executable");
        }
        catch (Exception exception)
        {
            ExtensionHost.LogMessage($"Helpers.SteamHelper.GetSteamLocation: {exception.Message}");
            throw;
        }
    }
    private static int AvailablePort
    {
        get
        {
            using var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
    internal static string GetAppType(int type)
    {
        return type switch
        {
            1 => Resources.appType_game,
            2 => Resources.appType_application,
            4 => Resources.appType_tool,
            8 => Resources.appType_demo,
            16 => Resources.appType_deprecatedMedia,
            32 => Resources.appType_dlc,
            64 => Resources.appType_guide,
            128 => Resources.appType_driver,
            256 => Resources.appType_config,
            512 => Resources.appType_hardware,
            1024 => Resources.appType_franchise,
            2048 => Resources.appType_video,
            4096 => Resources.appType_plugin,
            8192 => Resources.appType_music,
            16384 => Resources.appType_series,
            32768 => Resources.appType_comic,
            65536 => Resources.appType_beta,
            131072 => Resources.appType_legacyMedia,
            1073741824 => Resources.appType_shortcut,
            -2147483648 => Resources.appType_depotonly,
            _ => Resources.unknown
        };
    }
    internal static string GetAppDisplayStatus(int status)
    {
        return status switch
        {
            0 => Resources.displayStatus_invalid,
            1 => Resources.displayStatus_launching,
            2 => Resources.displayStatus_uninstalling,
            3 => Resources.displayStatus_installing,
            4 => Resources.displayStatus_running,
            5 => Resources.displayStatus_validating,
            6 => Resources.displayStatus_updating,
            7 => Resources.displayStatus_downloading,
            8 => Resources.displayStatus_synchronizing,
            9 => Resources.displayStatus_readyToInstall,
            10 => Resources.displayStatus_readyToPreload,
            11 => Resources.displayStatus_readyToLaunch,
            12 => Resources.displayStatus_regionRestricted,
            13 => Resources.displayStatus_presaleOnly,
            14 => Resources.displayStatus_invalidPlatform,
            15 => Resources.displayStatus_parentalBlocked,
            16 => Resources.displayStatus_preloadComplete,
            17 => Resources.displayStatus_borrowerLocked,
            18 => Resources.displayStatus_updatePaused,
            19 => Resources.displayStatus_updateQueued,
            20 => Resources.displayStatus_updateRequired,
            21 => Resources.displayStatus_updateDisabled,
            22 => Resources.displayStatus_downloadPaused,
            23 => Resources.displayStatus_downloadQueued,
            24 => Resources.displayStatus_downloadRequired,
            25 => Resources.displayStatus_downloadDisabled,
            26 => Resources.displayStatus_licensePending,
            27 => Resources.displayStatus_licenseExpired,
            28 => Resources.displayStatus_availForFree,
            29 => Resources.displayStatus_availToBorrow,
            30 => Resources.displayStatus_availGuestPass,
            31 => Resources.displayStatus_purchase,
            32 => Resources.displayStatus_unavailable,
            33 => Resources.displayStatus_notLaunchable,
            34 => Resources.displayStatus_cloudError,
            35 => Resources.displayStatus_cloudOutOfDate,
            36 => Resources.displayStatus_terminating,
            _ => Resources.unknown
        };
    }
}
