using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Gaku.ViewModels;

namespace Gaku.Views.Settings;

public partial class SettingsWindow : Window
{
    private SettingsWindowViewModel WindowViewModel => DataContext as SettingsWindowViewModel;
    public SettingsWindow()
    {
        InitializeComponent();
    }
}