using System;
using BerkutBot.Games.Game4.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game4.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame4Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game4Greetings>();
            services.AddTransient<IGameAnswer, Game4AnswerIncorrect>();

            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();
            return services;
        }
    }
}

