namespace MagicMirror.UniversalApp.ViewModels
{
    public struct MirrorVoiceCommand
    {
        public string VoiceCommand;
        public string TextSpoken;

        public MirrorVoiceCommand(string voiceCommand, string textSpoken)
        {
            VoiceCommand = voiceCommand;
            TextSpoken = textSpoken;
        }
    }
}