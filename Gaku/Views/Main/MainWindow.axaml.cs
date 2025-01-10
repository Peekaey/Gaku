using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Gaku.Interfaces;
using Gaku.Models;
using Gaku.ViewModels;
using Gaku.Views.Settings;

namespace Gaku.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
    public MainWindow()
    {
        InitializeComponent();
        
        // Subscribing to window events
        this.Opened += OnWindowOpened;
        this.SizeChanged += OnSizeChanged;
        
    }
    
    //Triggered when the window is opened (startup)
    private void OnWindowOpened(object? sender, EventArgs eventArgs)
    {
        var (width, height) = GetWindowSize();
        Console.WriteLine($"Window opened with width: {width}, height: {height}");
    }

    // Triggered when the window is resized - TBC when implementing custom screen size capture
    private void OnSizeChanged(object? sender, SizeChangedEventArgs eventArgs)
    {
        var newWidth = eventArgs.NewSize.Width;
        var newHeight = eventArgs.NewSize.Height;
        Console.WriteLine($"Window resized to width: {newWidth}, height: {newHeight}");
    }

    // Retrieve the current window size
    private (double Width, double Height) GetWindowSize()
    {
        return (this.Bounds.Width, this.Bounds.Height);
    }
    
    private async void CaptureFullscreenButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs eventArgs)
    {
        try
        {
            Console.WriteLine("Screenshot button clicked!");
            await ViewModel.Capture(this);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error when capturing screenshot in CaptureFullscreenButtonClick : {e.Message}");
        }
    }
    
    private async void CaptureRegionButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs eventArgs)
    {
        Console.WriteLine("Region button clicked!");
    }
    
    
    private async void OnSaveButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs eventArgs)
    {
        try
        {
            await ViewModel.SaveScreenshotToDisk(null);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving screenshot: {e.Message}");
        }
    }

    private async void OnSaveAsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs eventArgs)
    {
        try
        {
            var locationSavePrompt = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = "screenshot",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("PNG Image") { Patterns = new[] { "*.png" } },
                    new FilePickerFileType("JPEG Image") { Patterns = new[] { "*.jpg", "*.jpeg" } },
                    new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                }
            };

            var filePickerResult = await StorageProvider.SaveFilePickerAsync(locationSavePrompt);
            
            await ViewModel.SaveScreenshotToDisk(filePickerResult);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving screenshot: {e.Message}");
        }
    }
    
    private void OnSettingsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs eventArgs)
    {
        // TODO Improve this and optimise
        var appSettings = new AppSettings();
        var settingsViewModel = new SettingsWindowViewModel(appSettings);
        var settingsWindow = new SettingsWindow
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            DataContext = settingsViewModel
        };

        settingsWindow.ShowDialog(this);
        Console.WriteLine("Settings button clicked!");
    }

    private void OnHelpButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs eventArgs)
    {
        Console.WriteLine("Help button clicked!");
    }
}