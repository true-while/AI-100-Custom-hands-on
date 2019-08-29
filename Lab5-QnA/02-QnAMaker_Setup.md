# Lab 5: Creating a Customized QnA Maker Bot

## Lab 5.1: QnA Maker Setup

QnA Maker provides a conversational question and answer layer over your data. This allows your bot to send QnA Maker a question and receive an answer without you needing to parse and interpret the intent of their question.

One of the basic requirements in creating your own QnA Maker service is to seed it with questions and answers. In many cases, the questions and answers already exist in content like FAQs or other documentation; other times, you may want to customize your answers to questions in a more natural, conversational way.

### Prerequisites

- The code in this article is based on the QnA Maker [sample](https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore/11.qnamaker). Download C# sample. 
- Login in [QnA Maker account](https://www.qnamaker.ai/) with your Azure account.


### Scenario

In this hands-on you will build bot to utilize QnA Maker, you'll need to first create a knowledge base on QnA Maker, which we'll cover in the next section.
Your bot then can send it the user's query, and it will provide the best answer to the question in response.


## Lab 5.1 Create a QnA Maker service and publish a knowledge base

The first step is to create a QnA Maker service. Follow the steps to create the service in Azure.

1. Sign in to the [QnAMaker.ai](http://qnamaker.ai) portal with your Azure credentials.

1. On the QnA Maker portal, select **Create a knowledge base**.

1. On the Create page, in step 1, select Create a QnA service. You are directed to the Azure portal to set up a QnA Maker service in your subscription. If the Azure portal times out, select Try again on the site. After you connect, your Azure dashboard appears.

1. In QnA Maker, select the F0 tier and regions close to your location. Search pricing tier should be set to B with 15 indexes. When finish hit **Create** button. Wait until the QnA service will be created.

1. After you successfully create a new QnA Maker service in Azure, return to [QnAMaker.ai](http://qnamaker.ai). Select your QnA Maker service from the drop-down lists in step 2. If you just created a new QnA Maker service, be sure to refresh the page.

1. In step 3, name your knowledge base **My Sample QnA KB**.

1. To add content to your knowledge base, select three types of data sources. In step 4, under Populate your KB, add the [BitLocker Recovery FAQ](https://docs.microsoft.com/en-us/windows/security/information-protection/bitlocker/bitlocker-overview-and-requirements-faq) URL in the URL box.

1. Keep the rest by default and hit **Create** to create your KB.

1. While QnA Maker creates the knowledge base, a pop-up window appears. The extraction process takes a few minutes to read the HTML page and identify questions and answers.

1. After QnA Maker successfully creates the knowledge base, the Knowledge base page opens. You can edit the contents of the knowledge base on this page.

1. In the QnA Maker portal, on the Edit section, select Add QnA pair to add a new row to the knowledge base. Under Question, enter `Hi`. Under Answer, enter `Hello. Ask me BitLocker questions`. This will be used for discussion introduction.

1. In the upper right, select Save and train to save your edits and train the QnA Maker model. Edits aren't kept unless they're saved.

## Lab 5.2 Test the knowledge base

1. In the QnA Maker portal, in the upper right, select Test to test that the changes you made took effect. Enter `hi` there in the box, and select Enter. You should see the answer you created as a response.

1. Select **Inspect** to examine the response in more detail. The test window is used to test your changes to the knowledge base before they're published.

1. Select Test again to close the Test pop-up.

## Lab 5.3 Publish the knowledge base

When you publish a knowledge base, the question and answer contents of your knowledge base moves from the test index to a production index in Azure search.

In the QnA Maker portal, in the menu next to **Edit**, select **Publish**. Then to confirm, select **Publish** on the page.

The QnA Maker service is now successfully published. You can use the endpoint in your bot code. Do not close the QnA page yet.

When you make changes to the knowledge base and republish, you don't need to take further action with the bot. It's already configured to work with the knowledge base, and works with all future changes to the knowledge base. Every time you publish a knowledge base, all the bots connected to it are automatically updated.

## Lab 5.4 Create a bot service

1. In the QnA Maker portal, on the Publish page, select **Create bot**. This button appears only after you've published the knowledge base.

1. A new browser tab opens for the Azure portal, with the Azure Bot Service's creation page. Configure the Azure bot service.

Don't change the following settings in the Azure portal when creating the bot. They are pre-populated for your existing knowledge base:
- QnA Auth Key
- App service plan and location
- Azure Storage

1. The bot and QnA Maker can share the web app service plan, but can't share the web app. This means the app name must be different from the app name you used when you created the QnA Maker service.


## Lab 5.5 Testing a bot service

1. Now you can open created bot from Azure portal and select "Test in Web Chat".

1. Start testing form `hi` message and bot should return you `Hello. Ask me BitLocker questions`.

1. Next question you ask by query exact question from bitlocker [page](https://docs.microsoft.com/en-us/windows/security/information-protection/bitlocker/bitlocker-overview-and-requirements-faq). Type *What credentials are required to use BitLocker?* the answer should be start from *To turn on, turn off, or change configurations of B*

1. Now lets try to modify question to let QnA perform intent analyzing: "Are credentials required to set up Bitlocker?". Then answer should be the same. It means the QnA KB has close to intent answer.
