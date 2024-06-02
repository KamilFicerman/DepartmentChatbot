using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace DepartmentChatbot.Models
{
    public partial class Message : ObservableObject
    {
        public Message()
        {
            IsFAQ = false;
        }

        [field: NonSerialized]
        public string content;
        [JsonProperty("Content")]
        public string Content { get => content; set => SetProperty(ref content, value); }

        [field: NonSerialized]
        public string role;
        [JsonProperty("Role")]
        public string Role { get => role; set => SetProperty(ref role, value); }

        [field: NonSerialized]
        public bool isSent;
        [JsonProperty("IsSent")]
        [JsonIgnore]
        public bool IsSent { get => isSent; set => SetProperty(ref isSent, value); }

        [field: NonSerialized]
        public bool isFAQ;
        [JsonProperty("IsFAQ")]
        [JsonIgnore]
        public bool IsFAQ { get => isFAQ; set => SetProperty(ref isFAQ, value); }
    }
}
