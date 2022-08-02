using System;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game1.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame1Services(this IServiceCollection services)
        {

            services.AddTransient<IGameAnswer, Game1Answer8_9>();
            services.AddTransient<IGameAnswer, Game1AnswerRally>();
            services.AddTransient<IGameAnswer, Game1AnswerIncorrect>();
            services.AddTransient<IGameAnswer, Game1AnswerGreetings>();
            return services;
        }
    }
}

