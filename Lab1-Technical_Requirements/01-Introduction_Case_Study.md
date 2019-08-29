# Lab 1: Introducing Azure Cognitive Services

## Technical Requirements


In this lab, we will introduce our workshop case study and setup tools on your local workstation and in your Azure instance to enable you to build tools within the Microsoft Cognitive Services suite.

# Case Study
## Scenario

You've been assigned a new customer, Adventure Works LLC, which sells bicycles and bicycle equipment to its customers.

Adventure Works Cycles is a large multinational manufacturing company. It manufactures and sells bicycles and bicycle components to the North American, European and Asian commercial markets through both an internet channel and a reseller distribution network. Its base operations are in Kirkland, Washington with 290 employees, and there are several regional sales teams located throughout their market base.

Coming off a successful fiscal year, Adventure Works is looking to increase its revenue by targeting additional sales to their existing customers. In the last year, the marketing department started an initiative where they manually collected product ratings data for the Adventure Works products at various trade shows and racing events. This data is currently held in a Microsoft Excel file.

They have been able to prove that with the information collected, they are able to sell additional products to the existing client base through a manual analysis of the rating’s results. The Marketing team wanted to scale this by creating an online version of the survey on the Adventure Works website, but the Sales department heard of their effort and created an online survey. The take up of the survey on this platform has been very poor and the fields do not align with the data collected in the Excel files.

The Marketing department strongly believe that they can use the ratings data to make recommendations about other products as an upsell opportunity for the business. However, placing the survey in the website is proving to be a poor channel to harvest the data required and they are seeking advice on how this situation can be improved. In addition, the team are aware that performing the analysis manually will prove too difficult as more customers.

 Adventure Works aims to seamlessly scale up to handle large inquiry volumes of customers speaking various languages. Additionally, they wish to create a scalable customer service platform to gain more insight about customers' needs, problems, and product ratings.

In addition, the Customer Services department would like to offload some of their customer support function to an interactive platform. The intention is to reduce the workload on the staff and increase customer satisfaction by answer common questions quickly.

### Solution

The interactive platform is envisioned as a bot that will consist of the following functionality:

- Detect customer language (responding that only English is supported at this time)

- Monitor sentiment of the user

- Allow image uploading and determine if object is a bicycle

- Integrate FAQ into a chatbot

- Determine the user’s intentions based on entered text in the bot chat

- Log the chat bot session for later review

## Architecture

Your team recently presented a potential architecture (below) that Adventure Works approved:

![architecture](../images/AI_Immersion_Arch.png)


* [Text Analytics](https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics/) enables language detection
* [Computer Vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) allows uploading images, detects contents
* [QnA Maker](https://azure.microsoft.com/en-us/services/cognitive-services/qna-maker/) facilitating bot interactions from a static knowledge base
* [LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/Home)  (Language Understanding Intelligent Service)
extracts intent and entities from text
* [Azure Bot Service](https://azure.microsoft.com/en-us/services/bot-service/) connector service to enable chatbot interface to leverage app intelligence
