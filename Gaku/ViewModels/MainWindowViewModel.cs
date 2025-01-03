using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Gaku.Interfaces;

namespace Gaku.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IScreenCaptureService
{
    private readonly IScreenCaptureService _screenCaptureService;
    
    public MainWindowViewModel(IScreenCaptureService screenCaptureService)
    {
        _screenCaptureService = screenCaptureService;
    }
    
    public async Task CaptureAndSave(Window window, IStorageFile? filePath)
    {
        
        await _screenCaptureService.CaptureAndSave(window, filePath);
    }
    
}