using Azure.Storage.Blobs;
using BerkutBot.Games.Game1;
using BerkutBot.Games.Game1.Infrastructure;
using BerkutBot.Games.Game1.Options;
using BerkutBot.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
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
            builder.Services.Configure<Game1Options>(_functionConfig.GetSection("Game1Options"));
            builder.Services.AddLogging();
            builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(provider => 
                new TelegramBotClient(provider.GetService<IOptions<BotOptions>>().Value.Token));

            builder.Services.AddSingleton<BlobServiceClient>(provider => {
                BotOptions options = provider.GetRequiredService <IOptions<BotOptions>>().Value;
                return new BlobServiceClient(options.StorageBlobsConnectionString);
            });
            builder.Services.AddGame1Services();
        }
    }
}
