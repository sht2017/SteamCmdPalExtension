using Microsoft.CommandPalette.Extensions.Toolkit;
using SteamCmdPalExtension.Properties.Resources;
using System.Collections.Generic;
using System.IO;

namespace SteamCmdPalExtension;

public class Settings : JsonSettingsManager
{
    private static readonly string _namespace = "SteamPal";
    private static string Namespaced(string propertyName) => $"{_namespace}.{propertyName}";

    private static readonly List<ChoiceSetSetting.Choice> _debuggerModeChoices = [
        new(Resources.setting_debuggerMode_choice_tcp, "tcp"),
        new(Resources.setting_debuggerMode_choice_pipe, "pipe")
    ];

    private readonly TextSetting _maxResult = new(
        Namespaced(nameof(MaxResult)),
        Resources.setting_maxResult_description,
        Resources.setting_maxResult_label,
        "30");

    private readonly ChoiceSetSetting _debuggerMode = new(
        Namespaced(nameof(DebuggerMode)),
        Resources.setting_debuggerMode_description,
        Resources.setting_debuggerMode_label,
        _debuggerModeChoices);

    private readonly TextSetting _debuggerPort = new(
        Namespaced(nameof(DebuggerPort)),
        Resources.setting_debuggerPort_description,
        Resources.setting_debuggerPort_label,
        string.Empty);

    public int MaxResult => _maxResult.Value == null ?
            30 :
            int.TryParse(_maxResult.Value, out int result) ?
                result > 0 ?
                    result :
                    30 :
                30;
    public string DebuggerMode => _debuggerMode.Value ?? "tcp";
    public int? DebuggerPort => _debuggerPort.Value == null ?
            null :
            int.TryParse(_debuggerPort.Value, out int result) ?
                result > 0 && result < 65536 ?
                    result :
                    null :
                null;
    private static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);

        return Path.Combine(directory, "steampal.json");
    }

    public Settings()
    {
        FilePath = SettingsJsonPath();
        Settings.Add(_maxResult);
        Settings.Add(_debuggerMode);
        Settings.Add(_debuggerPort);
        LoadSettings();
        Settings.SettingsChanged += (s, a) => SaveSettings();
    }

}
