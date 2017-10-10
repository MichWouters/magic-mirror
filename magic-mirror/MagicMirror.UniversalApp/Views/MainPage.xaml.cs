using MagicMirror.UniversalApp.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; } = new MainPageViewModel();

        private DispatcherTimer rssTimerRefresh;

        public MainPage()
        {
            DataContext = ViewModel;
            InitializeComponent();

            rssTimerRefresh = new DispatcherTimer();
            SetUpTimer(rssTimerRefresh, new TimeSpan(0, 0, 5), Scroll);
        }

        private void Scroll(object sender ,object e)
        {
            double pixelsToScroll = Fml.FontSize * 2;
            if (Fml.VerticalOffset + pixelsToScroll + 10 <= RSSTextBlock.ActualHeight)
            {
                Fml.ChangeView(0, Fml.VerticalOffset + pixelsToScroll, 1);
            }
            else
            {
                Fml.ChangeView(0, 0, 1);
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
            ViewModel.NavigateToSettings();
        }

        private void TemperatureTextBlock_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ViewModel.NavigateToOfflineData();
        }
    }
}