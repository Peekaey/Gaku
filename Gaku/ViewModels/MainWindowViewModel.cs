using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Gaku.Interfaces;
using Gaku.Models;

namespace Gaku.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly IScreenCaptureService _screenCaptureService;
    private readonly ISystemNotificationService _systemNotificationService;
    private readonly IFileService _fileService;
    private readonly IOSPlatformHelper _osPlatformHelper;
    public MainWindowViewModel(IScreenCaptureService screenCaptureService, ISystemNotificationService systemNotificationService, 
        IMacOSGraphics macOSGraphics, IFileService fileService, IOSPlatformHelper osPlatformHelper)
    {
        _screenCaptureService = screenCaptureService;
        _systemNotificationService = systemNotificationService;
        _fileService = fileService;
        _osPlatformHelper = osPlatformHelper;
    }
    
    private Bitmap? _capturedImage;

    public Bitmap? CapturedImage
    {
        get => _capturedImage;
        set
        {
            if (_capturedImage != value)
            {
                _capturedImage = value;
                OnPropertyChanged(nameof(CapturedImage));
                Console.WriteLine("CapturedImage property changed.");
            }
        }
    }
    
    // TODO Move System Notification into its own handler away from ViewModel
    public async Task Capture(Window window)
    {
        try
        {
            var fullScreenBitmap = _osPlatformHelper.CaptureScreenshot();
            if (fullScreenBitmap != null)
            {

                CapturedImage = fullScreenBitmap;
            }
            else
            {
                Console.WriteLine("Capture returned a null Bitmap.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error capturing screenshot: {ex.Message}");
        }
    }
    
    public async Task SaveScreenshotToDisk(IStorageFile? filePath)
    {
        if (CapturedImage == null)
        {
            Console.WriteLine("No image to save.");
        }
        else
        {
            var result = await _fileService.SaveScreenshotToDisk(CapturedImage, filePath);

            if (result.Success)
            {
                _systemNotificationService.ShowMacOSNotificationViaOSAScript("Gaku",
                    $"Screenshot saved to {result.SavePath}");
            }
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        Console.WriteLine($"Property changed: {propertyName} to {GetType().GetProperty(propertyName)?.GetValue(this)}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}