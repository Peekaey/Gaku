using Avalonia.Media.Imaging;

namespace Gaku.Interfaces;

public interface IOSPlatformHelper
{
    Bitmap? CaptureScreenshot();
}