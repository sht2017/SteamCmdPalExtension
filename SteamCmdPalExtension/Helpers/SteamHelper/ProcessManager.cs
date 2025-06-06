using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.SteamHelper;
internal sealed class ProcessManager
{
    internal static bool Kill(Process process, bool killTree = false)
    {
        try
        {
            process.Kill(killTree);
            process.WaitForExit();
            process.Dispose();
            return true;
        }
        catch (Exception exception)
        {
            ExtensionHost.LogMessage($"SteamHelper.ProcessManager.Kill(Process): {exception.Message}");
            return false;
        }
    }

    internal static bool Kill(int pid, bool killTree = false)
    {
        try
        {
            Process process = Process.GetProcessById(pid);
            return Kill(process, killTree);
        }
        catch (Exception exception)
        {
            ExtensionHost.LogMessage($"SteamHelper.ProcessManager.Kill(int): {exception.Message}");
            return false;
        }
    }

    private static string Get(string processName)
    {
        if (string.IsNullOrEmpty(processName))
        {
            return string.Empty;
        }
        if (processName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            return processName[..^4];
        }
        return processName;
    }
    internal static bool Kill(string processName, bool killTree = false)
    {
        try
        {
            Process[] processes = Process.GetProcessesByName(Get(processName));
            if (processes.Length == 0)
            {
                return false;
            }
            bool result = false;
            foreach (Process process in processes)
            {
                result |= Kill(process, killTree);
            }
            return result;
        }
        catch (Exception exception)
        {
            ExtensionHost.LogMessage($"SteamHelper.ProcessManager.Kill(string): {exception.Message}");
            return false;
        }
    }
    internal static void Start(string filename, string arguments)
    {
        using Process process = new();
        process.StartInfo = new()
        {
            FileName = filename,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        process.Start();
    }
    internal static void WaitFor(string processName, int interval = 50)
    {
        while (Process.GetProcessesByName(Get(processName)).Length == 0)
            Thread.Sleep(interval);
    }
    internal async static Task WaitForAsync(string processName, int interval = 50)
    {
        while (Process.GetProcessesByName(Get(processName)).Length == 0)
            await Task.Delay(interval).ConfigureAwait(false);
    }
}
