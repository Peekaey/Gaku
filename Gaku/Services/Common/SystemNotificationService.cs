using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Avalonia.Controls.Notifications;
using Gaku.Interfaces;


namespace Gaku.Service;

public class SystemNotificationService : ISystemNotificationService
{
    // Temp using OSAScript - Highly not recommended but useful for development testing
    // Move to a more modern API on MacOS
    public void ShowMacOSNotificationViaOSAScript(string title, string message)
    {
        try
        {
            // Escape double quotes and backslashes
            string escapedTitle = EscapeOsaString(title);
            string escapedMessage = EscapeOsaString(message);

            var process = new ProcessStartInfo
            {
                FileName = "/usr/bin/osascript",
                Arguments = $"-e \"display notification \\\"{escapedMessage}\\\" with title \\\"{escapedTitle}\\\"\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(process);
            proc?.WaitForExit();

            if (proc?.ExitCode != 0)
            {
                string error = proc?.StandardError.ReadToEnd() ?? "Unknown error";
                Console.WriteLine($"Failed to show notification: {error}");
            }
            else
            {
                Console.WriteLine("Notification displayed successfully on macOS.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while showing notification: {ex.Message}");
        }
    }
    private string EscapeOsaString(string input)
    {
        return Regex.Replace(input, @"[\\\""]", "\\$0");
    }
}