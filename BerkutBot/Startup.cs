using System;
using System.Net.Http;
using Azure.Identity;
using BerkutBot.Games.Game10.Infrastructure;
using BerkutBot.Infrastructure;
using BerkutBot.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
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
            builder.Services.Configure<SchedulerOptions>(_functionConfig.GetSection("SchedulerOptions"));
            builder.Services.AddLogging();
            builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(provider => 
                new TelegramBotClient(provider.GetRequiredService<IOptions<BotOptions>>().Value.Token));

            builder.Services.AddAzureClients(clients => {
                clients.UseCredential(new DefaultAzureCredential());
                clients.AddBlobServiceClient(_functionConfig.GetSection("Storage"));
            });


            builder.Services
                .AddHttpClient<IAnnouncementScheduler, AnnouncementScheduler>((provider, client) => {
                    var options = provider.GetRequiredService<IOptions<SchedulerOptions>>();
                    client.BaseAddress = options.Value.BaseAddress;
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            builder.Services.AddSingleton<IGameAnswerFactory, GameAnswerFactory>();
            builder.Services.AddGameCommonServices();
            builder.Services.AddGame10Services();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
