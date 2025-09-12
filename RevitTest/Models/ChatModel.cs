using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitTest.Models
{
    public class ContentItem
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }

    public class ChatResponse
    {
        [JsonProperty("messages")]
        public List<ChatResponseMessage> Messages { get; set; } = new List<ChatResponseMessage>();

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class ChatResponseMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("contents")]
        public List<ChatResponseContent> Contents { get; set; } = new List<ChatResponseContent>();
    }

    public class ChatResponseContent
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

}
