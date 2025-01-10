namespace Gaku.Interfaces;

public interface ISystemNotificationService
{
    void ShowMacOSNotificationViaOSAScript(string title, string message);
}