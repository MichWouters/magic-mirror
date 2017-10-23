using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.VoiceCommands;

namespace MagicMirror.UniversalApp.Services
{
    public sealed class MirrorVoiceCommandService : IBackgroundTask
    {
        private VoiceCommandServiceConnection voiceServiceConnection;
        private VoiceCommandServiceConnection voiceCommandServiceConnection;
        private BackgroundTaskDeferral serviceDeferral;
        private ResourceMap cortanaResourceMap;
        private ResourceContext cortanaContext;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            cortanaResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            cortanaContext = ResourceContext.GetForViewIndependentUse();

            if (triggerDetails != null && triggerDetails.Name == "MirrorVoiceCommandService")
            {
                try
                {
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceCommandServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;
                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();

                    switch (voiceCommand.CommandName)
                    {
                        case "openSettings":
                            await SendCompletionMessageForSettings();
                            break;
                        default:
                            LauncAppInForeground();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Handling Voice Command failed " + ex.ToString());
                }
            }
        }

        private async Task SendCompletionMessageForSettings()
        {
            string openingSettings = "";
            await ShowProgressScreen(openingSettings);
        }

        private async void LauncAppInForeground()
        {
            var userMessage = new VoiceCommandUserMessage
            {
                SpokenMessage = "Opening Magic Mirror"
            };

            var response = VoiceCommandResponse.CreateResponse(userMessage);
            response.AppLaunchArgument = "";

            await voiceServiceConnection.RequestAppLaunchAsync(response);
        }

        private void OnVoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            if (serviceDeferral != null)
            {
                serviceDeferral.Complete();
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            System.Diagnostics.Debug.WriteLine("Task cancelled, clean up");
            if (serviceDeferral != null)
            {
                //Complete the service deferral
                serviceDeferral.Complete();
            }
        }

        private async Task ShowProgressScreen(string message)
        {
            var userProgressMessage = new VoiceCommandUserMessage();
            userProgressMessage.DisplayMessage = userProgressMessage.SpokenMessage = message;

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userProgressMessage);
            await voiceServiceConnection.ReportProgressAsync(response);
        }
    }
}
