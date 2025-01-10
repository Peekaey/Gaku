using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Gaku.Helpers;
using Gaku.Interfaces;
using Gaku.Models;
using Gaku.Service;
using Gaku.ViewModels;
using Gaku.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Gaku;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;
    private StyleInclude _lightTheme;
    private StyleInclude _darkTheme;
    
    public override void Initialize()
    {

        AvaloniaXamlLoader.Load(this);
        
        
        _lightTheme = new StyleInclude(new Uri("avares://Gaku/"))
        {
            Source = new Uri("avares://Gaku/Themes/LightTheme.axaml")
        };
        
        _darkTheme = new StyleInclude(new Uri("avares://Gaku/"))
        {
            Source = new Uri("avares://Gaku/Themes/DarkTheme.axaml")
        };
        
        SetTheme(Themes.Dark);
        
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
     
        //Register Services
        services.AddSingleton<IScreenCaptureService, ScreenCaptureService>();
        services.AddSingleton<ISystemNotificationService, SystemNotificationService>();
        services.AddSingleton<IMacOSGraphics, MacOSGraphics>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IOSPlatformHelper, OSPlatformHelper>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SettingsWindowViewModel>();
        services.AddSingleton<AppSettings>();

        _serviceProvider = services.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            // DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
            Console.WriteLine("OSVersion: " + Environment.OSVersion);
        }
        base.OnFrameworkInitializationCompleted();
    }

    public void SetTheme(Themes themes)
    {
        // Removes the current theme
        if (Styles.Contains(_lightTheme))
        {
            Styles.Remove(_lightTheme);
        }

        if (Styles.Contains(_darkTheme))
        {
            Styles.Remove(_darkTheme);
        }
        
        if (themes == Themes.Light)
        {
            Styles.Add(_lightTheme);
        }
        else if (themes == Themes.Dark)
        {
            Styles.Add(_darkTheme);
        }
        
    }
}