using Azure.Identity;
using Azure.Storage.Blobs;
using BerkutBot.Games.Game1;
using BerkutBot.Games.Game1.Infrastructure;
using BerkutBot.Infrastructure;
using BerkutBot.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

[assembly: FunctionsStartup(typeof(BerkutBot.Startup))]

namespace BerkutBot
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();


            builder.Services.Configure<BotOptions>(_functionConfig.GetSection("BotOptions"));
            builder.Services.AddLogging();
            builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(provider => 
                new TelegramBotClient(provider.GetRequiredService<IOptions<BotOptions>>().Value.Token));

            builder.Services.AddAzureClients(clients => {
                clients.UseCredential(new DefaultAzureCredential());
                clients.AddBlobServiceClient(_functionConfig.GetSection("Storage"));
            });
            builder.Services.AddSingleton<IGameAnswerFactory, GameAnswerFactory>();
            builder.Services.AddGame1Services();
        }
    }
}
