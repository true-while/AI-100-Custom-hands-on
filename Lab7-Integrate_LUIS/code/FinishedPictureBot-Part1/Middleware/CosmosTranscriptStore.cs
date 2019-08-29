using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples.Middleware
{
    public class CosmosTranscriptStore : ITranscriptLogger
    {
        private CosmosDbStorage _storage;

        public CosmosTranscriptStore(CosmosDbStorageOptions config)
        {
            _storage = new CosmosDbStorage(config);
        }
        public async Task LogActivityAsync(IActivity activity)
        {
            // activity only contains Text if this is a message
            var isMessage = activity.AsMessageActivity() != null ? true : false;
            if (isMessage)
            {
                // Customize this to save whatever data you want
                var data = new
                {
                    From = activity.From,
                    To = activity.Recipient,
                    Text = activity.AsMessageActivity().Text,
                };
                var document = new Dictionary<string, object>();
                // activity.Id is being used as the Cosmos Document Id
                document.Add(activity.Id, data);
                await _storage.WriteAsync(document, new CancellationToken());
            }
        }
    }
}
