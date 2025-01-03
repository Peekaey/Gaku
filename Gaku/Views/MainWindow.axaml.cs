using System;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Gaku.Interfaces;
using Gaku.ViewModels;

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
    private void OnWindowOpened(object? sender, EventArgs e)
    {
        var (width, height) = GetWindowSize();
        Console.WriteLine($"Window opened with width: {width}, height: {height}");
    }

    // Triggered when the window is resized - TBC when implementing custom screen size capture
    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var newWidth = e.NewSize.Width;
        var newHeight = e.NewSize.Height;
        Console.WriteLine($"Window resized to width: {newWidth}, height: {newHeight}");
    }

    // Retrieve the current window size
    private (double Width, double Height) GetWindowSize()
    {
        return (this.Bounds.Width, this.Bounds.Height);
    }
    
    private async void OnTakeScreenshotButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("Screnshot button clicked!");
        await ViewModel.CaptureAndSave(this, null);
        
    }
    
    private void OnSaveButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("Save button clicked!");
    }

    private async void OnSaveAsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var locationSavePrompt = new FilePickerSaveOptions
        {
            Title = "Save As",
            SuggestedFileName = "screenshot.png",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("PNG Image") { Patterns = new[] { "*.png" } },
                new FilePickerFileType("JPEG Image") { Patterns = new[] { "*.jpg", "*.jpeg" } },
                new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
            }
        };

        var result = await StorageProvider.SaveFilePickerAsync(locationSavePrompt);
        
        Console.WriteLine("Save as button clicked!");
    }
    
    private void OnSettingsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("Settings button clicked!");
    }

    private void OnHelpButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("Help button clicked!");
    }
}