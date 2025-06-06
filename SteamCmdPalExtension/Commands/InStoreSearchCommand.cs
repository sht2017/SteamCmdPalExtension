using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Diagnostics;

namespace SteamCmdPalExtension.Commands;

internal sealed partial class InStoreSearchCommand : InvokableCommand
{
    private readonly string _content;
    internal InStoreSearchCommand(string content)
    {
        _content = content;
    }
    public override ICommandResult Invoke()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = $"steam://openurl/{new Uri($"https://store.steampowered.com/search/?term={Uri.EscapeDataString(_content)}").AbsoluteUri}",
            UseShellExecute = true,
            CreateNoWindow = true
        });
        return CommandResult.Dismiss();
    }
}