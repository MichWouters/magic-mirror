using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;
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

        private SettingsService settingsService;

        public MirrorVoiceCommandService()
        {
            settingsService = new SettingsService();
        }

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

                            CompletionMessage message = new CompletionMessage
                            {
                                Message = $"Change your name to { name}?",
                                RepeatMessage = $"Do you want to change your name to {name}?",
                                ConfirmMessage = $"Changing name to {name}",
                                CompletedMessage = $"Your Magic Mirror name has been changed to {name}",
                                CanceledMessage = "Keeping name to original value",
                            };

                            await SendCompletionMessage(ParameterAction.ChangeName, name, message);
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

        private async Task SendCompletionMessage(ParameterAction action, string parameter, CompletionMessage completionMessage)
        {
            var userMessage = new VoiceCommandUserMessage { DisplayMessage = completionMessage.Message, SpokenMessage = completionMessage.Message };
            var userRepeatMessage = new VoiceCommandUserMessage { DisplayMessage = completionMessage.RepeatMessage, SpokenMessage = completionMessage.RepeatMessage };

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponseForPrompt(userMessage, userRepeatMessage);
            VoiceCommandConfirmationResult confirmation = await voiceServiceConnection.RequestConfirmationAsync(response);

            if (confirmation != null)
            {
                if (confirmation.Confirmed)
                {
                    await ShowProgressScreen(completionMessage.ConfirmMessage);

                    switch (action)
                    {
                        case ParameterAction.ChangeName:
                            ChangeName(parameter);
                            break;

                        case ParameterAction.ChangeTemperature:
                            break;

                        case ParameterAction.ChangeDistance:
                            break;

                        default:
                            break;
                    }

                    // Provide a completion message to the user.
                    var nameChangedMessage = new VoiceCommandUserMessage { DisplayMessage = completionMessage.CompletedMessage, SpokenMessage = completionMessage.CompletedMessage };

                    response = VoiceCommandResponse.CreateResponse(nameChangedMessage);
                    await voiceServiceConnection.ReportSuccessAsync(response);
                }
                else
                {
                    // Confirm no action for the user.
                    var cancelledMessage = new VoiceCommandUserMessage();
                    cancelledMessage.DisplayMessage = cancelledMessage.SpokenMessage = completionMessage.CanceledMessage;

                    response = VoiceCommandResponse.CreateResponse(cancelledMessage);
                    await voiceServiceConnection.ReportSuccessAsync(response);
                }
            }
        }

        private void ChangeName(string name)
        {
            string localFolder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string SETTING_FILE = "settings.json";

            UserSettings userSettings = settingsService.ReadSettings(localFolder, SETTING_FILE);
            userSettings.UserName = name;
            settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
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