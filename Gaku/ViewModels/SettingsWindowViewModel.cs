using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Gaku.Models;

namespace Gaku.ViewModels;

public class SettingsWindowViewModel : ViewModelBase , INotifyPropertyChanged
{
    private readonly AppSettings _appSettings;
    
    public SettingsWindowViewModel(AppSettings appSettings)
    {
        _appSettings = appSettings;
        // Populate available themes
        Themes = Enum.GetValues(typeof(Themes)).Cast<Themes>().ToList();
    }
    
    // Gets properties on Initialisation from AppSettings
    public string DefaultFolderPath
    {
        get => _appSettings.DefaultFolderPath;
        set
        {
            if (_appSettings.DefaultFolderPath != value)
            {
                _appSettings.DefaultFolderPath = value;
                OnPropertyChanged(nameof(DefaultFolderPath));
            }
        }
    }

    public bool AutoSaveToDefaultFolderPath
    {
        get => _appSettings.AutoSaveToDefaultFolderPath;
        set
        {
            if (_appSettings.AutoSaveToDefaultFolderPath != value)
            {
                _appSettings.AutoSaveToDefaultFolderPath = value;
                OnPropertyChanged(nameof(AutoSaveToDefaultFolderPath));
            }
        }
    }
    
    public bool AutoSaveToClipboard
    {
        get => _appSettings.AutoSaveToClipboard;
        set
        {
            if (_appSettings.AutoSaveToClipboard != value)
            {
                _appSettings.AutoSaveToClipboard = value;
                OnPropertyChanged(nameof(AutoSaveToClipboard));
            }
        }
    }

    public List<Themes> Themes { get; }

    public Themes SelectedTheme
    {
        get => _appSettings.SelectedThemes;
        set
        {
            if (_appSettings.SelectedThemes != value)
            {
                _appSettings.SelectedThemes = value;
                // Dynamically apply the new theme
                var app = (App)Application.Current;
                app.SetTheme(value);
                OnPropertyChanged(nameof(SelectedTheme));

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