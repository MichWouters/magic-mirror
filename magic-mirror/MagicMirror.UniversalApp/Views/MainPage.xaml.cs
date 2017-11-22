using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer rssTimerRefresh;

        public MainPage()
        {
            InitializeComponent();

            rssTimerRefresh = new DispatcherTimer();
            SetUpTimer(rssTimerRefresh, new TimeSpan(0, 0, 5), Scroll);
        }

        private void Scroll(object sender, object e)
        {
            double pixelsToScroll = scrollingRss.FontSize * 2;
            if (scrollingRss.VerticalOffset + pixelsToScroll + 10 <= RSSTextBlock.ActualHeight)
            {
                scrollingRss.ChangeView(0, scrollingRss.VerticalOffset + pixelsToScroll, 1);
            }
            else
            {
                scrollingRss.ChangeView(0, 0, 1);
            }
        }

        private void SetUpTimer(DispatcherTimer timer, TimeSpan timeSpan, EventHandler<object> method)
        {
            timer.Tick += method;
            timer.Interval = timeSpan;
            if (!timer.IsEnabled) timer.Start();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.NavigationService.Navigate<SettingPage>();
        }

        private void TemperatureTextBlock_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            App.NavigationService.Navigate<OfflineDataPage>();
        }

        private async void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, new TimeSpan(1)));
        }
    }
}