using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TranslatorAPIToken
    {
        private static readonly Uri srvUrl = new Uri(Translator.CognitiveServicesTokenUri);
        private static readonly TimeSpan cacheDuration = new TimeSpan(0, 3, 0);

        private string _tokenValue = "";
        private DateTime _tokenTime = DateTime.MinValue;

        public string APIKey { get; }

        public TranslatorAPIToken(string key)
        {
            APIKey = key;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if ((DateTime.Now - _tokenTime) < cacheDuration)
            {
                return _tokenValue;
            }

            HttpRequestMessage request = null;
            HttpClient client = null;

            try
            {
                client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(180)
                };
                request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = srvUrl,
                    Content = new StringContent(string.Empty),
                };
                request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", APIKey);

                var response = await client.SendAsync(request);
                 response.EnsureSuccessStatusCode();

                var token = await response.Content.ReadAsStringAsync();
                _tokenTime = DateTime.Now;
                _tokenValue = $"Bearer {token}";

                return _tokenValue;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (client != null) client.Dispose();
                if (request != null) client.Dispose();
            }

        }
    }
}