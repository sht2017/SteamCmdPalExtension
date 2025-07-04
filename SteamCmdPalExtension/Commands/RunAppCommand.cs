using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Properties.Resources;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Commands;

internal sealed partial class RunAppCommand : InvokableCommand
{
    private readonly long _appid;
    internal RunAppCommand(long appid)
    {
        _appid = appid;
        Name = Resources.play;
    }
    public override ICommandResult Invoke()
    {
        Task.Run(() =>
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"steam://launch/{_appid}/dialog",
                UseShellExecute = true,
                CreateNoWindow = true
            });
        });
        return CommandResult.Dismiss();
    }
}
