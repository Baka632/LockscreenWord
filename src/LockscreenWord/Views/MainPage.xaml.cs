// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

using LockscreenWord.Common;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;

namespace LockscreenWord.Views;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class MainPage : Page
{
    private bool isFirstRun;
    private bool isTitleBarTextBlockForwardBegun;

    public MainPage()
    {
        this.InitializeComponent();

        ConfigureTitleBar();
        AutoSetMainPageBackground();

        ContentFrameNavigationHelper = new NavigationHelper(ContentFrame);
        ContentFrameNavigationHelper.Navigate(typeof(MainFramePage));
    }

    private void ConfigureTitleBar()
    {
        if (EnvironmentHelper.IsWindowsMobile)
        {
            TitleBarTextBlock.Visibility = Visibility.Collapsed;
        }
        else
        {
            TitleBarHelper.Hook(ContentFrame);
            TitleBarHelper.BackButtonVisibilityChanged += OnBackButtonVisibilityChanged;
            TitleBarHelper.TitleBarVisibilityChanged += OnTitleBarVisibilityChanged;
        }
    }

    private void AutoSetMainPageBackground()
    {
        if (MicaHelper.IsSupported())
        {
            Background = new SolidColorBrush(Colors.Transparent);
            MicaHelper.TrySetMica(this);
        }
        else if (AcrylicHelper.IsSupported())
        {
            AcrylicHelper.TrySetAcrylicBrush(this);
        }
        else
        {
            Background = Resources["ApplicationPageBackgroundThemeBrush"] as Brush;
        }
    }

    private void OnTitleBarVisibilityChanged(object sender, CoreApplicationViewTitleBar bar)
    {
        if (bar.IsVisible)
        {
            StartTitleBarAnimation(Visibility.Visible);
        }
        else
        {
            StartTitleBarAnimation(Visibility.Collapsed);
        }
    }

    private void OnBackButtonVisibilityChanged(object sender, AppViewBackButtonVisibility args)
    {
        StartTitleTextBlockAnimation(args);
    }

    private void StartTitleTextBlockAnimation(AppViewBackButtonVisibility buttonVisibility)
    {
        switch (buttonVisibility)
        {
            case AppViewBackButtonVisibility.Disabled:
            case AppViewBackButtonVisibility.Visible:
                if (isTitleBarTextBlockForwardBegun)
                {
                    goto default;
                }
                TitleBarTextBlockForward.Begin();
                isTitleBarTextBlockForwardBegun = true;
                break;
            case AppViewBackButtonVisibility.Collapsed:
                TitleBarTextBlockBack.Begin();
                isTitleBarTextBlockForwardBegun = false;
                break;
            default:
                break;
        }
    }

    private void StartTitleBarAnimation(Visibility visibility)
    {
        if (isFirstRun)
        {
            isFirstRun = false;
            TitleBar.Visibility = visibility;
            return;
        }
        TitleBar.Visibility = visibility;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TileContent content = TileCreationHelper.CreateLockscreenWordTile("啊我死了", "佩佩！", "占位！");
        XmlDocument document = content.GetXml();

        TileNotification notification = new(document);

        TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
    }
}
