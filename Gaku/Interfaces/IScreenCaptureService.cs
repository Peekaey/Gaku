using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Gaku.Interfaces;

public interface IScreenCaptureService
{
    Task CaptureAndSave(Window window, IStorageFile? filePath);
}