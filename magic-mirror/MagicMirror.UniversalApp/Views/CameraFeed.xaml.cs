using MagicMirror.UniversalApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MagicMirror.UniversalApp.Views
{
    public sealed partial class CameraFeed : UserControl
    {
        public CameraFeedViewModel ViewModel { get; } = new CameraFeedViewModel();
        public CameraFeed()
        {
            DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}
