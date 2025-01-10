using Avalonia.Media.Imaging;

namespace Gaku.Interfaces;

public interface IMacOSGraphics
{
    Bitmap CaptureFullScreen();
}