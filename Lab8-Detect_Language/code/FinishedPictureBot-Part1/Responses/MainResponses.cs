using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;

namespace Microsoft.BotBuilderSamples.Responses
{
    public class MainResponses
    {
  
        public static async Task ReplyWithGreeting(ITurnContext context, Func<string, string> translate)
        {
            // Add a greeting
            await context.SendActivityAsync(translate.Invoke($"Hi, I'm PictureBot!"));
        }
        public static async Task ReplyWithHelp(ITurnContext context, Func<string, string> translate)
        {
            await context.SendActivityAsync(translate.Invoke($"I can search for pictures, share pictures and order prints of pictures."));
        }
        public static async Task ReplyWithResumeTopic(ITurnContext context, Func<string, string> translate)
        {
            await context.SendActivityAsync(translate.Invoke($"What can I do for you?"));
        }
        public static async Task ReplyWithConfused(ITurnContext context, Func<string, string> translate)
        {
            // Add a response for the user if Regex or LUIS doesn't know
            // What the user is trying to communicate
            await context.SendActivityAsync(translate.Invoke($"I'm sorry, I don't understand."));
        }
        public static async Task ReplyWithLuisScore(ITurnContext context, string key, double score)
        {
            await context.SendActivityAsync($"Intent: {key} ({score}).");
        }
        public static async Task ReplyWithShareConfirmation(ITurnContext context, Func<string, string> translate)
        {
            await context.SendActivityAsync(translate.Invoke($"Posting your picture(s) on twitter..."));
        }
        public static async Task ReplyWithOrderConfirmation(ITurnContext context, Func<string, string> translate)
        {
            await context.SendActivityAsync(translate.Invoke($"Ordering standard prints of your picture(s)..."));
        }
        public static async Task ReplyWithSearchingConfirmation(ITurnContext context, Func<string, string> translate)
        {
            await context.SendActivityAsync(translate.Invoke($"I'm searching for your picture(s)..."));
        }
    }
}