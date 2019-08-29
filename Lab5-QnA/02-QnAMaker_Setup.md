# Lab 5: Creating a Customized QnA Maker Bot

##  Introduction

In this lab we will explore the QnA Maker for creating bots that connect to a pre-trained knowledgebase.  Using QnAMaker, you can upload documents or point to web pages and have it pre-populate a knowledge base that can feed a simple bot for common uses such as frequently asked questions.

## Lab 5.1: QnA Maker Setup

1.  Open the [Azure Portal](https://portal.azure.com)

1.  Click **Add a new resource**

1.  Type **QnA Maker**, select **QnA Maker**

1.  Click **Create**

1.  Type a name

1.  Select the **S0** tier for the resource pricing tier.  We aren't using the free tier because we will upload files that are larger than 1MB later.

1.  Select your resource group

1.  For the search pricing tier, select the **F** tier

1.  Enter an appname, it must be unique

1.  Click **Create**.  This will created the following resources in your resource group:

-   App Service Plan
-   App Service
-   Application Insights
-   Search Service
-   Cognitive Service instance of type QnAMaker
-   Web App

## Lab 5.2: Create a KnowledgeBase

1.  Open the [QnA Maker site](https://qnamaker.ai)

1.  Login using the Azure credentials for your new QnA Maker resource

1.  Click **Create a knowledge base**

1.  Skip step 1 as you have already created the resource

1.  Select your Azure AD and Subscription tied to your QnA Maker resource, then select your newly created QnA Maker resource

1.  For the name, type **Microsoft FAQs***

1.  For the file, click **Add file**, browse to the **code/surface-pro-4-user-guide-EN.pdf** file

1.  For the file, click **Add file**, browse to the **code/Manage Azure Blob Storage** file

> **Note** You can find out more about the supported file types and data sources [here](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/concepts/data-sources-supported)

1.  For the **Chit-chat**, select **Witty**

1.  Click **Create your KB**

## Lab 5.3: Publish and Test your Knowledge base

1.  Review the knowledge base QnA pairs, you should see ~200 different pairs based on the two documents we fed it

1.  In the top menu, click **PUBLISH**.  

1.  On the publish page, click **Publish**.  Once the service is published, click the **Create Bot** button on the success page

1.  On the bot service creation page, fix any naming errors, then click **Create**.

> **Note**  Recent change in Azure requires dashes ("-") be removed from some resource names

1.  Once the bot resource is created, navigate to the new **Web App Bot**, then click **Test in Web Chat**

1.  Ask the bot any questions related to a Surface Pro 4 or managing Azure Blog Storage:

+   How do I add memory?
+   How long does it take to charge the battery?
+   How do I hard reset?
+   What is a blob?

1.  Ask it some questions it doesn't know, such as:

+   How do I bowl a strike?

## Lab 5.4: Download the Bot Source code

1.  Click the **Build** tab

1.  Click **Download Bot source code**, when prompted click **Yes**.  

1.  Azure will build your source code, when complete, click **Download Bot source code**

1.  Extract the zip file to your local computer

1.  Open the solution file, Visual Studio will open

1.  Open the **Startup.cs** file, you will notice nothing special has been added here

1.  Open the **Bots/{BotName}.cs** file, notice this is where the QnA Maker is being initalized:

```csharp
var qnaMaker = new QnAMaker(new QnAMakerEndpoint
{
    KnowledgeBaseId = _configuration["QnAKnowledgebaseId"],
    EndpointKey = _configuration["QnAAuthKey"],
    Host = GetHostname()
},
null,
httpClient);
```

and then called:

```csharp
var response = await qnaMaker.GetAnswersAsync(turnContext);
if (response != null && response.Length > 0)
{
    await turnContext.SendActivityAsync(MessageFactory.Text(response[0].Answer), cancellationToken);
}
else
{
    await turnContext.SendActivityAsync(MessageFactory.Text("No QnA Maker answers were found."), cancellationToken);
}
```

As you can see, it is very simple to add a generated QnA Maker to your own bots with just a few lines of code.

## Going Further

What would you do to integrate the QnAMaker code into your picture bot?

##  Resources

-   [What is the QnA Maker service?](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/overview/overview)

##  Next Steps

-   [Lab 06-01: Implement LUIS](../Lab6-Implement_LUIS/01-Introduction.md)