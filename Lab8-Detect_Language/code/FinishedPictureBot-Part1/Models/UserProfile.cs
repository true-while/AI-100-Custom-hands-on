using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples.Models
{
    public class UserProfile
    {
        public string Greeted { get; set; } = "not greeted";
        public List<string> UtteranceList { get; private set; } = new List<string>();
        public string Language { get; set; }
    }


    public class ConversationData
    {
        public string ChannelId { get; set; }
        public string Timestamp { get; set; }
        public bool PromptedUserForName { get; set; }

    }
}
