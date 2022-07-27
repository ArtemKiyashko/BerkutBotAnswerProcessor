using System;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game1.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame1Services(this IServiceCollection services)
        {
            services.AddSingleton<IGame1AnswerFactory, Game1AnswerFactory>();
            services.AddTransient<IGame1Answer, Game1Answer8_9>();
            services.AddTransient<IGame1Answer, Game1AnswerRally>();
            services.AddTransient<IGame1Answer, Game1AnswerIncorrect>();
            services.AddTransient<IGame1Answer, Game1AnswerGreetings>();
            services.AddSingleton<IUpdateMessageFactory, UpdateMessageFactory>();
            return services;
        }
    }
}

