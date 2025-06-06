using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Properties.Resources;
using System.Diagnostics;

namespace SteamCmdPalExtension.Commands;

internal sealed partial class AppPropertiesCommand : InvokableCommand
{
    private readonly int _appid;

    internal AppPropertiesCommand(int appid)
    {
        _appid = appid;
        Name = Resources.properties;
    }
    public override ICommandResult Invoke()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = $"steam://gameproperties/{_appid}",
            UseShellExecute = true,
            CreateNoWindow = true
        });
        return CommandResult.Dismiss();
    }
}