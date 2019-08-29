#Lab 8 - Detect Language

In this lab we are going to integrate language detection ability of cognitive services into our bot.

## Lab 8.1: Retrieve your Cognitive Services url and keys

1.  Open the [Azure Portal](https://portal.azure.com)

1.  Navigate to your resource group, select the cognitive services resource that is generic (aka, it contains all end points).

1.  Click **Keys**, record the url and the key for the cognitive services resource

##  Lab 8.2: Add language support to your bot

1.  If not already open, open your PictureBot solution

1.  Right-click the project and select **Manage Nuget Packages**

1.  Click **Browse**

1.  Search for **Microsoft.Azure.CognitiveServices.Language.TextAnalytics**, select it then click **Install**, then click **I Accept**

1.  Open the **Startup.cs** file, add the following using statements:

```csharp
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
```

1.  Add the following code to the **ConfigureServices** method:

```csharp
services.AddSingleton(sp =>
{
    string cogsBaseUrl = Configuration.GetSection("cogsBaseUrl")?.Value;
    string cogsKey = Configuration.GetSection("cogsKey")?.Value;

    var credentials = new ApiKeyServiceClientCredentials(cogsKey);
    TextAnalyticsClient client = new TextAnalyticsClient(credentials)
    {
        Endpoint = cogsBaseUrl
    };

    return client;
});
```

1.  Open the **PictureBot.cs** file, add the following using statements:

```csharp
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
```

1.  Add the following class variable:

```csharp
private TextAnalyticsClient _textAnalyticsClient;
```

1.  Modify the constructor to include the new TextAnalyticsClient:

```csharp
public PictureBot(PictureBotAccessors accessors, ILoggerFactory loggerFactory,LuisRecognizer recognizer, TextAnalyticsClient analyticsClient)
```

1.  Inside the constructor, initialize the class variable:

```csharp
_textAnalyticsClient = analyticsClient;
```

1.  Navigate to the **OnTurnAsync** method and find the following line of code:

```csharp
var utterance = turnContext.Activity.Text;
var state = await _accessors.PictureState.GetAsync(turnContext, () => new PictureState());
state.UtteranceList.Add(utterance);
await _accessors.ConversationState.SaveChangesAsync(turnContext);
```

1.  Add the following line of code after it

```csharp
//Check the language
var result = _textAnalyticsClient.DetectLanguage(turnContext.Activity.Text);

switch (result.DetectedLanguages[0].Name)
{
    case "English":
        break;
    default:
        //throw error
        await turnContext.SendActivityAsync($"I'm sorry, I can only understand English. [{result.DetectedLanguages[0].Name}]");
        return;
        break;
}
```

1.  Open the **appsettings.json** file and ensure that your cognitive services settings are entered:

```csharp
"cogsBaseUrl": "",
"cogsKey" :  ""
```

1.  Press **F5** to start your bot

1.  Using the Bot Emulator, send in a few phrases and see what happens:

+   Como Estes?
+   Bon Jour!
+   Привет
+   Hello

## Going further

Since we have already introduced you to LUIS in previous labs, think about what changes you may need to make to support multiple languages using LUIS.  Some helpful articles:

-   [Language and region support for LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-language-support)   

##  Resources

-   [Example: Detect language with Text Analytics](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/how-tos/text-analytics-how-to-language-detection)
-   [Quickstart: Text analytics client library for .NET](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/quickstarts/csharp)

## Next Steps

-   [Lab 09-01: Test Bot DirectLine](../Lab9-Test_Bots_DirectLine/01-Introduction.md)