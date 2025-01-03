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
    private readonly AppSettings _appSettings;
    
    public ScreenCaptureService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    // TODO Separate Save & Capture functions
    public async Task CaptureAndSave(Window window, IStorageFile? filePath)
    {
        var savePath = "";
        if (filePath == null)
        {
            savePath = Path.Combine(_appSettings.DefaultFolderPath, $"screenshot - {DateTime.Now}.png");
        }
        else
        {
            savePath = filePath.Path.AbsolutePath;
        }
        
        var pixelSize = new PixelSize((int)window.Width, (int)window.Height);
        var size = new Size(window.Width, window.Height);
        var dpi = new Vector(96, 96); // Default Screen DPI (look at custom scaling at a later date)
        
        var renderTargetBitmap = new RenderTargetBitmap(pixelSize, dpi);        {
            renderTargetBitmap.Render(window);
            
            //Render the window's visual tree
            renderTargetBitmap.Render(window);
            
            // Save the rendered bitmap to a file
            using (var fileStream = File.Create(savePath))
            {
                renderTargetBitmap.Save(fileStream);
            }
        }
    }
}