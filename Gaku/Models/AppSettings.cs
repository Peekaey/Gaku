using System;
using Avalonia.Controls.Shapes;

namespace Gaku.Models;

public class AppSettings
{
    public string DefaultFolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    
}