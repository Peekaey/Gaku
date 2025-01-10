using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Gaku.Models;

namespace Gaku.Interfaces;

public interface IFileService
{
    Task<FileSaveResult> SaveScreenshotToDisk(Bitmap imageBitmap, IStorageFile? storageFile);
}