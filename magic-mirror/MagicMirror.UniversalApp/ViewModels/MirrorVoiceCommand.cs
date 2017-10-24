namespace MagicMirror.UniversalApp.ViewModels
{
    public struct MirrorVoiceCommand
    {
        public string VoiceCommand;
        public string CommandMode;
        public string TextSpoken;
        public string Burger;

        public MirrorVoiceCommand(string voiceCommand, string commandMode, string textSpoken, string burger)
        {
            VoiceCommand = voiceCommand;
            CommandMode = commandMode;
            TextSpoken = textSpoken;
            Burger = burger;
        }
    }
}