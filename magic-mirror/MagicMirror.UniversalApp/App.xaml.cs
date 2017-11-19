using MagicMirror.UniversalApp.Common;
using MagicMirror.UniversalApp.Services;
using MagicMirror.UniversalApp.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MagicMirror.UniversalApp
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        public static NavigationService NavigationService { get; private set; }
        private RootFrameNavigationHelper rootFrameNavigationHelper;

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                NavigationService = new NavigationService(rootFrame);
                rootFrameNavigationHelper = new RootFrameNavigationHelper(rootFrame);
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                //Check if manual or Cortana activation
                if (string.IsNullOrEmpty(e.Arguments))
                {
                    // Launching normally
                    rootFrame.Navigate(typeof(MainPage), "");
                }
                else
                {
                    // Cortana launching
                    rootFrame.Navigate(typeof(SettingPage), e.Arguments);
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            try
            {
                // Load Vcd Storage File
                StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync(@"MirrorCommands.xml");

                // Install Voice commands from file
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);

                await UpdatePhraseList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }
        }

        private async Task UpdatePhraseList()
        {
            try
            {
                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue("MirrorCommandSet", out VoiceCommandDefinition commandDefinition))
                {
                    await commandDefinition.SetPhraseListAsync("", null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Updating Phrase list for VCDs: " + ex.ToString());
            }
        }

        private void SetUpUnityContainers()
        {
            IUnityContainer myContainer = new UnityContainer();
            myContainer.RegisterType<ILocationService, LocationService>();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            Type navigationToPageType;
            ViewModels.MirrorVoiceCommand? navigationCommand = null;

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var commandArgs = args as VoiceCommandActivatedEventArgs;
                SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

                string voiceCommandName = speechRecognitionResult.RulePath[0];
                string textSpoken = speechRecognitionResult.Text;

                switch (voiceCommandName)
                {
                    case "openSettings":
                        navigationCommand = new ViewModels.MirrorVoiceCommand(voiceCommandName, textSpoken);
                        navigationToPageType = typeof(SettingPage);
                        break;

                    case "openOfflineSettings":
                        navigationCommand = new ViewModels.MirrorVoiceCommand(voiceCommandName, textSpoken);
                        navigationToPageType = typeof(OfflineDataPage);
                        break;

                    case "openMain":
                        navigationCommand = new ViewModels.MirrorVoiceCommand(voiceCommandName, textSpoken);
                        navigationToPageType = typeof(MainPage);
                        break;

                    default:
                        navigationToPageType = typeof(MainPage);
                        break;
                }
            }
            else
            {
                navigationToPageType = typeof(MainPage);
            }

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                NavigationService = new NavigationService(rootFrame);

                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(navigationToPageType, navigationCommand);
            Window.Current.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }
    }
}