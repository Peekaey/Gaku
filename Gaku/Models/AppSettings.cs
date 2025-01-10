using System;
using Avalonia.Controls.Shapes;

namespace Gaku.Models;

public class AppSettings
{
    // Expand depending on OS
    public string DefaultFolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    public bool AutoSaveToDefaultFolderPath { get; set; } = true;
    public bool AutoSaveToClipboard { get; set; } = false;
    public Themes SelectedThemes { get; set; } = Themes.Dark;
    public OperatingSystem CurrentOS { get; set; } = Environment.OSVersion;
    
}