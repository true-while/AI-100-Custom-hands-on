# Lab 7: Integrate LUIS into Bot Dialogs

> Prerequisites: This lab builds on [Lab 2](../Lab2-Basic_Filter_Bot/02-Basic_Filter_Bot.md).
It is recommended that you do that lab in order to be able to implement logging as covered in this lab. If you have not, reading carefully through all the exercises and looking at some of the code or using it in your own applications may be sufficient, depending on your needs.

Our bot is now capable of taking in a user's input and responding based on the user's input. Unfortunately, our bot's communication skills are brittle. One typo, or a rephrasing of words, and the bot will not understand. This can cause frustration for the user. We can greatly increase the bot's conversational abilities by enabling it to understand natural language with the LUIS model we built in [Lab 4](../Lab4-Implement_LUIS/02-Implement_LUIS.md)"

We will have to update our bot in order to use LUIS.  We can do this by modifying "Startup.cs" and "PictureBot.cs."

## Lab 7.1: Adding natural language understanding

### Adding LUIS to Startup.cs

Open "Startup.cs" and locate the `ConfigureServices` method. We'll add LUIS here by adding an additional service for LUIS after creating and registering the state accessors.

Below:
```csharp
            services.AddSingleton((Func<IServiceProvider, PictureBotAccessors>)(sp =>
            {
                .
                .
                .
                return accessors;
            });
```

Add:
```csharp
            // Create and register a LUIS recognizer.
            services.AddSingleton(sp =>
            {
                // Get LUIS information
                var luisApp = new LuisApplication( "YourLuisAppId", "YourLuisKey", "YourLuisEndpoint");

                // Specify LUIS options. These may vary for your bot.
                var luisPredictionOptions = new LuisPredictionOptions
                {
                    IncludeAllIntents = true,
                };

                // Create the recognizer
                var recognizer = new LuisRecognizer(luisApp, luisPredictionOptions, true,null);
                return recognizer;
            });
```

Update the `luisApp` with the app ID, subscription key, and base URI for your LUIS model. The base URI will be "https://region.api.cognitive.microsoft.com/" (do include the final forward slash, and URL protocol specification), where region is the region associated with the key you are using. Some examples of regions are, `westus`, `westcentralus`, `eastus2`, and `southeastasia`.

You can find your base URL by logging into www.luis.ai, going to the **Publish** tab, and looking at the **Endpoint** column under **Resources and Keys**. The base URL is the portion of the **Endpoint URL** before 'luis' and the other parameters.

**Hint**: The LUIS App ID will have hyphens in it, and the LUIS subscription key will not.

## Lab 7.2: Adding LUIS to PictureBot's MainDialog

Open "PictureBot.cs." The first thing you'll need to do is initialize the LUIS recognizer, similar to how you did for `PictureBotAccessors`. Below the commented line `// Initialize LUIS Recognizer`, add the following:

```csharp
private LuisRecognizer _recognizer { get; } = null;
```

Next, navigate to where you initialize a new instance of the `PictureBot` class. The first line will look like this in your code:

```csharp
public PictureBot(PictureBotAccessors accessors, ILoggerFactory loggerFactory /*, LuisRecognizer recognizer*/)
```

Now, maybe you noticed we had this commented out in your previous labs, maybe you didn't. You have it commented out now, because up until now, you weren't calling LUIS, so a LUIS recognizer didn't need to be an input to `PictureBot`. Now, we are using the recognizer.

 Uncomment the input requirement (parameter `LuisRecognizer recognizer`), and add the following line below `// Initialize LUIS Recognizer`:


```csharp
_recognizer = recognizer ?? throw new ArgumentNullException(nameof(recognizer));
```

Again, this should look very similar to how we initialized the instance of `_accessors`.

As far as updating our `MainDialog` goes, there's no need for us to add anything to the initial `GreetingAsync` step, because regardless of user input, we want to greet the user when the conversation starts.

In `MainMenuAsync`, we do want to start by trying Regex, so we'll leave most of that. However, if Regex doesn't find an intent, we want the `default` action to be different. That's when we want to call LUIS.

Within the `MainMenuAsync` switch block, replace:

```csharp
 default:
     {
        await MainResponses.ReplyWithConfused(stepContext.Context);
        return await stepContext.EndDialogAsync();
     }
```

With:

```csharp
default:
{
    // Call LUIS recognizer
    var result = await _recognizer.RecognizeAsync(stepContext.Context, cancellationToken);
    // Get the top intent from the results
    var topIntent = result?.GetTopScoringIntent();
    // Based on the intent, switch the conversation, similar concept as with Regex above
    switch ((topIntent != null) ? topIntent.Value.intent : null)
    {
          case null:
                                // Add app logic when there is no result.
                await MainResponses.ReplyWithConfused(stepContext.Context);
                break;
          case "None":
                await MainResponses.ReplyWithConfused(stepContext.Context);
                // with each statement, we're adding the LuisScore, purely to test, so we know whether LUIS was called or not
                 await MainResponses.ReplyWithLuisScore(stepContext.Context, topIntent.Value.intent, topIntent.Value.score);
                break;
          case "Greeting":
                await MainResponses.ReplyWithGreeting(stepContext.Context);
                await MainResponses.ReplyWithHelp(stepContext.Context);
                await MainResponses.ReplyWithLuisScore(stepContext.Context, topIntent.Value.intent, topIntent.Value.score);
                break;
          case "OrderPic":
                await MainResponses.ReplyWithOrderConfirmation(stepContext.Context);
                await MainResponses.ReplyWithLuisScore(stepContext.Context, topIntent.Value.intent, topIntent.Value.score);
                break;
          case "SharePic":
                await MainResponses.ReplyWithShareConfirmation(stepContext.Context);
                await MainResponses.ReplyWithLuisScore(stepContext.Context, topIntent.Value.intent, topIntent.Value.score);
                break;
          case "SearchPics":
                await MainResponses.ReplyWithSearchingConfirmation(stepContext.Context);
                await MainResponses.ReplyWithLuisScore(stepContext.Context, topIntent.Value.intent, topIntent.Value.score);
                break;
          default:
                await MainResponses.ReplyWithConfused(stepContext.Context);
                break;
    }
    return await stepContext.EndDialogAsync();
}
```

Now we add new function `ReplyWithSearchingConfirmation` to `MainResponses.cs`

```csharp
public static async Task ReplyWithSearchingConfirmation(ITurnContext context)
{
    await context.SendActivityAsync($"I'm searching for your picture(s)...");
}
```

Let's briefly go through what we're doing in the new code additions. First, instead of responding saying we don't understand, we're going to call LUIS. So we call LUIS using the LUIS Recognizer, and we store the Top Intent in a variable. We then use `switch` to respond in different ways, depending on which intent is picked up. This is almost identical to what we did with Regex.

> Note: If you named your intents differently in LUIS than instructed in the code accompanying [Lab 6](../Lab6-Implement_LUIS/02-Implement_LUIS.md), you need to modify the `case` statements accordingly.

Another thing to note is that after every response that called LUIS, we're adding the LUIS intent value and score. The reason is just to show you when LUIS is being called as opposed to Regex (you would remove these responses from the final product, but it's a good indicator for us as we test the bot).

## Lab 7.3: Testing natural speech phrases
Hit F5 to run the app. In the Bot Emulator, try sending the bots different ways of searching pictures. What happens when you say "send me pictures of water" or "show me dog pics"? Try some other ways of asking for, sharing and ordering pictures.

If you have extra time, see if there are things LUIS isn't picking up on that you expected it to. Maybe now is a good time to go to luis.ai, [review your endpoint utterances](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/label-suggested-utterances), and retrain/republish your model.

> Fun Aside: Reviewing the endpoint utterances can be extremely powerful.  LUIS makes smart decisions about which utterances to surface.  It chooses the ones that will help it improve the most to have manually labeled by a human-in-the-loop.  For example, if the LUIS model predicted that a given utterance mapped to Intent1 with 47% confidence and predicted that it mapped to Intent2 with 48% confidence, that is a strong candidate to surface to a human to manually map, since the model is very close between two intents.

## Going further
If you're having trouble customizing your LUIS implementation, review the documentation guidance for adding LUIS to bots [here](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-v4-luis?view=azure-bot-service-4.0&tabs=cs).

>Get stuck or broken? You can find the solution for the lab up until this point under [code/FinishedPictureBot-Part1](./code/FinishedPictureBot-Part1). You will need to insert the keys for your Azure Bot Service in the `appsettings.json` file. We recommend using this code as a reference, not as a solution to run, but if you choose to run it, be sure to add the necessary keys (in this section, there shouldn't be any).

**Extra Credit**

If you wish to attempt to integrate LUIS bot including Azure Search, building on the prior supplementary LUIS model-with-search [training] (https://github.com/Azure/LearnAI-Bootcamp/tree/master/lab01.5-luis), follow the following trainings: [Azure Search](https://github.com/Azure/LearnAI-Bootcamp/tree/master/lab02.1-azure_search), and [Azure Search Bots](https://github.com/Azure/LearnAI-Bootcamp/blob/master/lab02.2-building_bots/2_Azure_Search.md).
