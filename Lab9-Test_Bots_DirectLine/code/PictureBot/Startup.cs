// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Azure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.BotBuilderSamples.Bots;
using Microsoft.Bot.Builder.EchoBot;
using Microsoft.Extensions.Options;
using Microsoft.Bot.Builder.Integration;
using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Configuration;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.BotBuilderSamples.Middleware;
using Microsoft.BotBuilderSamples.Models;

namespace Microsoft.BotBuilderSamples
{
    public class Startup
    {
        private const string BotOpenIdMetadataKey = "BotOpenIdMetadata";
        private bool _isProduction = false;
        private ILoggerFactory _loggerFactory;


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, PictureBot>();

            //init bot
            services.AddBot<PictureBot>(options =>
            {
                //read appsettings.json
                var secretKey = Configuration.GetSection("MicrosoftAppId")?.Value;
                var botFilePath = Configuration.GetSection("MicrosoftAppPassword")?.Value;

                options.CredentialProvider = new SimpleCredentialProvider(secretKey, botFilePath);

            });


            services.AddSingleton<PictureBotAccessors>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BotFrameworkOptions>>().Value;
                if (options == null)
                {
                    throw new InvalidOperationException("BotFrameworkOptions must be configured prior to setting up the state accessors");
                }
                                              
                  //read Cosmos DB settings from appsettings.json
                  CosmosDbStorage _myStorage = new CosmosDbStorage(new CosmosDbStorageOptions
                   {
                       AuthKey = Configuration.GetSection("CosmosDB").GetValue<string>("Key"),
                       CollectionId = Configuration.GetSection("CosmosDB").GetValue<string>("CollectionName"),
                       CosmosDBEndpoint = new Uri(Configuration.GetSection("CosmosDB").GetValue<string>("EndpointURI")),
                       DatabaseId = Configuration.GetSection("CosmosDB").GetValue<string>("DatabaseName"),
                   });

                var conversationState = new ConversationState(_myStorage);
                var userState = new UserState(_myStorage);

                // Create the custom state accessor.
                // State accessors enable other components to read and write individual properties of state.
                var accessors = new PictureBotAccessors(conversationState, userState)
                {
                    PictureState = conversationState.CreateProperty<PictureState>(PictureBotAccessors.PictureStateName),
                    DialogStateAccessor = conversationState.CreateProperty<DialogState>("DialogState"),
                };

                return accessors;
            });


            // Create and register a LUIS recognizer.
            services.AddSingleton(sp =>
            {
                // Get LUIS information
                var luisApp = new LuisApplication(
                    "...", 
                    "...",
                    "https://westus.api.cognitive.microsoft.com/");

                // Specify LUIS options. These may vary for your bot.
                var luisPredictionOptions = new LuisPredictionOptions
                {
                    IncludeAllIntents = true,
                };

                // Create the recognizer
                var recognizer = new LuisRecognizer(luisApp, luisPredictionOptions, true, null);
                return recognizer;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();
            
        }
    }
}
