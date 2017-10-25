using System;
using System.Resources;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.VoiceCommands;

namespace VoiceCommandService
{
    public sealed class MirrorVoiceCommandService : IBackgroundTask
    {
        private VoiceCommandServiceConnection voiceServiceConnection;
        private BackgroundTaskDeferral serviceDeferral;
        private ResourceMap cortanaResourceMap;
        private ResourceContext cortanaContext;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            cortanaContext = ResourceContext.GetForViewIndependentUse();

            if (triggerDetails != null && triggerDetails.Name == "MirrorVoiceCommandService")
            {
                try
                {
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;
                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();

                    switch (voiceCommand.CommandName)
                    {
                        case "changeName":
                            string name = voiceCommand.Properties["name"][0];
                            await SendCompletionMessageForChangeName(name);
                            break;

                        default:
                            LaunchAppInForeground();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Handling Voice Command failed " + ex.ToString());
                }
            }
        }

        private async Task SendCompletionMessageForChangeName(string name)
        {
            var userPrompt = new VoiceCommandUserMessage();
            VoiceCommandResponse response;

            var userMessage = new VoiceCommandUserMessage
            {
                DisplayMessage = $"Change your name to {name}?",
                SpokenMessage = $"Change your name to {name}?"
            };

            response = VoiceCommandResponse.CreateResponseForPrompt(userMessage, userPrompt);
            var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);
        }

        private async void LaunchAppInForeground()
        {
            var userMessage = new VoiceCommandUserMessage
            {
                SpokenMessage = "Opening Magic Mirror at this time"
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