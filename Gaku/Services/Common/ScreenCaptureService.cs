using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Gaku.Interfaces;
using Gaku.Models;

namespace Gaku.Service;

public class ScreenCaptureService : IScreenCaptureService
{
    private readonly IMacOSGraphics _macOsGraphics;
    
    public ScreenCaptureService(IMacOSGraphics macOsGraphics)
    {
        _macOsGraphics = macOsGraphics;
    }
    
    public Bitmap CaptureMacOSFullScreen()
    {
        var bitmap = _macOsGraphics.CaptureFullScreen();
        return bitmap;
    }
}
