using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Gaku.Interfaces;
using Gaku.Models;

namespace Gaku.Service;

public class FileService : IFileService
{
    private readonly AppSettings _appSettings;

    public FileService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task<FileSaveResult> SaveScreenshotToDisk(Bitmap imageBitmap, IStorageFile? storageFile)
    {
        try
        {
            var savePath = GetSavePath(storageFile);

            await Task.Run(() =>
            {
                using var fileStream = File.Create(savePath);
                imageBitmap.Save(fileStream);
            });
            
            return FileSaveResult.AsFileSaveSuccess(savePath);
        }
        catch (Exception e)
        {
            return FileSaveResult.AsFileSaveFailure(e.Message);
        }
    }
    
    private string GetSavePath(IStorageFile? storagePath)
    {
        var savePath = "";

        if (storagePath == null)
        {
            var fileName = $"screenshot - {DateTime.Now.ToString("HH-mm-dd.MM.yyyy")}.png";
            savePath = Path.Combine(_appSettings.DefaultFolderPath, fileName);
        }
        else
        {
            savePath = storagePath.Path.AbsolutePath;
        }
        return savePath;
    }
}
