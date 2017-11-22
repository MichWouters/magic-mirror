using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;

namespace VoiceCommandService
{
    public sealed class MirrorVoiceCommandService : IBackgroundTask
    {
        private VoiceCommandServiceConnection voiceServiceConnection;
        private BackgroundTaskDeferral serviceDeferral;
        private SettingsService settingsService;
        private readonly string localFolder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        private const string SETTING_FILE = "settings.json";

        public MirrorVoiceCommandService()
        {
            settingsService = new SettingsService();
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;
            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails != null && triggerDetails.Name == "MirrorVoiceCommandService")
            {
                try
                {
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;
                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
                    CompletionMessage message;

                    switch (voiceCommand.CommandName)
                    {
                        case "changeName":
                            string name = voiceCommand.Properties["name"][0];

                            message = new CompletionMessage
                            {
                                Message = $"Change your name to {name}?",
                                RepeatMessage = $"Do you want to change your name to {name}?",
                                ConfirmMessage = $"Changing name to {name}",
                                CompletedMessage = $"Your Magic Mirror name has been changed to {name}",
                                CanceledMessage = "Keeping name to original value",
                            };
                            await SendCompletionMessage(ParameterAction.ChangeName, name, message);
                            break;

                        case "changeAddress":
                            string address = voiceCommand.Properties["address"][0];

                            message = new CompletionMessage
                            {
                                Message = $"Change your address to {address}?",
                                RepeatMessage = $"Do you want to change your address to {address}?",
                                ConfirmMessage = $"Changing address to {address}",
                                CompletedMessage = $"Your address has been changed to {address}",
                                CanceledMessage = "Keeping address to original value",
                            };
                            await SendCompletionMessage(ParameterAction.ChangeAddress, address, message);
                            break;

                        case "changeTown":
                            string town = voiceCommand.Properties["town"][0];

                            message = new CompletionMessage
                            {
                                Message = $"Change your home town to {town}?",
                                RepeatMessage = $"Do you want to change your home town to {town}?",
                                ConfirmMessage = $"Changing home town to {town}",
                                CompletedMessage = $"Your home town has been changed to {town}",
                                CanceledMessage = "Keeping town to original value",
                            };
                            await SendCompletionMessage(ParameterAction.ChangeTown, town, message);
                            break;

                        case "changeWorkAddress":
                            string workAddress = voiceCommand.Properties["workAddress"][0];

                            message = new CompletionMessage
                            {
                                Message = $"Change your work Address to {workAddress}?",
                                RepeatMessage = $"Do you want to change your work Address to {workAddress}?",
                                ConfirmMessage = $"Changing work Address to {workAddress}",
                                CompletedMessage = $"Your workAddress changed to {workAddress}",
                                CanceledMessage = "Keeping workAddress to original value",
                            };
                            await SendCompletionMessage(ParameterAction.ChangeWorkAddress, workAddress, message);
                            break;

                        case "changeTemperature":
                            string temperature = voiceCommand.Properties["temperature"][0];

                            message = new CompletionMessage
                            {
                                Message = $"Change your temperature notation to {temperature}?",
                                RepeatMessage = $"Do you want to hange your temperature notation to {temperature}?",
                                ConfirmMessage = $"Changing temperature notation to {temperature}",
                                CompletedMessage = $"Your temperature notation changed to {temperature}",
                                CanceledMessage = "Keeping temperature to original value",
                            };
                            await SendCompletionMessage(ParameterAction.ChangeTemperature, temperature, message);
                            break;

                        case "changeDistance":
                            string distance = voiceCommand.Properties["distance"][0];

                            message = new CompletionMessage
                            {
                                Message = $"Change your distance notation to {distance} system?",
                                RepeatMessage = $"Do you want to hange your distance notation to {distance} system?",
                                ConfirmMessage = $"Changing distance notation to {distance} system",
                                CompletedMessage = $"Your temperature notation changed to {distance}",
                                CanceledMessage = "Keeping distance to original system",
                            };
                            await SendCompletionMessage(ParameterAction.ChangeDistance, distance, message);
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
                            ChangeTemperature(parameter);
                            break;

                        case ParameterAction.ChangeDistance:
                            ChangeDistance(parameter);
                            break;

                        case ParameterAction.ChangeAddress:
                            ChangeAddress(parameter);
                            break;

                        case ParameterAction.ChangeTown:
                            ChangeTown(parameter);
                            break;

                        case ParameterAction.ChangeWorkAddress:
                            ChangeWorkAddress(parameter);
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

        private void ChangeAddress(string address)
        {
            UserSettings userSettings = settingsService.ReadSettings(localFolder, SETTING_FILE);
            userSettings.HomeAddress = address;
            settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
        }

        private void ChangeTown(string address)
        {
            UserSettings userSettings = settingsService.ReadSettings(localFolder, SETTING_FILE);
            userSettings.HomeCity = address;
            settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
        }

        private void ChangeWorkAddress(string address)
        {
            UserSettings userSettings = settingsService.ReadSettings(localFolder, SETTING_FILE);
            userSettings.WorkAddress = address;
            settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
        }

        private void ChangeDistance(string distance)
        {
            UserSettings userSettings = settingsService.ReadSettings(localFolder, SETTING_FILE);
            Enum.TryParse(distance, out DistanceUOM myTemp);
            userSettings.DistanceUOM = myTemp;
            settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
        }

        private void ChangeTemperature(string temperature)
        {
            UserSettings userSettings = settingsService.ReadSettings(localFolder, SETTING_FILE);
            Enum.TryParse(temperature, out TemperatureUOM myTemp);
            userSettings.TemperatureUOM = myTemp;
            settingsService.SaveSettings(localFolder, SETTING_FILE, userSettings);
        }

        private void ChangeName(string name)
        {
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