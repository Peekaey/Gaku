using System;
using Avalonia.Media.Imaging;
using Gaku.Interfaces;
using Gaku.Models;

namespace Gaku.Helpers;

public class OSPlatformHelper : IOSPlatformHelper
{
    private readonly AppSettings _appSettings;
    private readonly IScreenCaptureService _screenCaptureService;
    private readonly ISystemNotificationService _systemNotificationService;
    
    public OSPlatformHelper(AppSettings appSettings, IScreenCaptureService screenCaptureService, ISystemNotificationService systemNotificationService)
    {
        _appSettings = appSettings;
        _screenCaptureService = screenCaptureService;
        _systemNotificationService = systemNotificationService;
    }
    
    private OS GetPlatform()
    {
        if (_appSettings.CurrentOS.ToString().Contains("Unix"))
        {
            return OS.MacOS;
        }
        else if (_appSettings.CurrentOS.ToString().Contains("Windows"))
        {
            return OS.Windows;
        }
        else if (_appSettings.CurrentOS.ToString().Contains("Linux"))
        {
            return OS.Linux;
        }
        else
        {
            return OS.Unknown;
        }
    }


    // TODO Set as Nullable for now till all OS specific implementations are done
    public Bitmap? CaptureScreenshot()
    {
        switch (GetPlatform())
        {
            case OS.MacOS:
                var bitmap = _screenCaptureService.CaptureMacOSFullScreen();
                _systemNotificationService.ShowMacOSNotificationViaOSAScript("Gaku", "Screenshot captured successfully!");
                return bitmap;
            case OS.Windows:
                return null;
            case OS.Linux:
                return null;
            case OS.Unknown:
                return null;
            default:
                return null;
        }
    }
}