using System;

namespace RevitTest.Models
{
    public class ChatMessage
    {
        public string Text { get; set; }
        public bool IsUserMessage { get; set; }
        public DateTime Timestamp { get; set; }

        public ChatMessage(string text, bool isUserMessage)
        {
            Text = text;
            IsUserMessage = isUserMessage;
            Timestamp = DateTime.Now;
        }
    }
}