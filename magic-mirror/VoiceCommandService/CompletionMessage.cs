namespace VoiceCommandService
{
    public sealed class CompletionMessage
    {
        public string Message { get; set; }
        public string RepeatMessage { get; set; }
        public string ConfirmMessage { get; set; }
        public string CompletedMessage { get; set; }
        public string CanceledMessage { get; set; }
    }
}