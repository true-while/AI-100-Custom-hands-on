using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class Translator
    {
        string _token;
        public static string TranslatorUri = "https://api.microsofttranslator.com/v2/http.svc/";
        public static string CognitiveServicesTokenUri = "https://XXX.cognitiveservices.azure.com/sts/v1.0/issuetoken";
        public static string SubscriptionKey = "XXX";

        public Translator()
        {
            _token = Task.Run(GetBearerTokenForTranslator).Result;
        }

        internal async Task<string> GetBearerTokenForTranslator()
        {
            var azureSubscriptionKey = SubscriptionKey;
            var azureAuthToken = new TranslatorAPIToken(azureSubscriptionKey);
            return await azureAuthToken.GetAccessTokenAsync();
        }

        public string Translate(string input, string inputLang, string outputLang)
        {
            try
            {
                return ReadRespons($"{TranslatorUri}Translate?text={HttpUtility.UrlEncode(input)}&from={inputLang}&to={outputLang}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string Detect(string input)
        {
            try
            {
                return ReadRespons($"{TranslatorUri}Detect?text=" + HttpUtility.UrlEncode(input));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private string ReadRespons(string uri)
        {
            WebRequest translationWebRequest = WebRequest.Create(uri);
            translationWebRequest.Headers.Add("Authorization", _token);

            using (WebResponse response = translationWebRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    Encoding encode = Encoding.GetEncoding("UTF-8");
                    using (StreamReader translatedStream = new StreamReader(stream, encode))
                    {
                        XmlDocument xTranslation = new XmlDocument();
                        xTranslation.LoadXml(translatedStream.ReadToEnd());
                        return xTranslation.InnerText;
                    }
                }
            }
        }
    }
}