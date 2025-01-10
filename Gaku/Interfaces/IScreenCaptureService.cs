using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace Gaku.Interfaces;

public interface IScreenCaptureService
{
    Bitmap CaptureMacOSFullScreen();
}