using LockscreenWord.ViewModels;

namespace LockscreenWord.Views;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class MainFramePage : Page
{
    public MainFrameViewModel ViewModel { get; } = new MainFrameViewModel();

    public MainFramePage()
    {
        this.InitializeComponent();
    }
}
