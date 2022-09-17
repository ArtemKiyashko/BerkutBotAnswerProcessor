using System;
using BerkutBot.Games.Game3.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game3.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame3Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game3Greetings>();
            services.AddTransient<IGameAnswer, Game3AnswerIncorrect>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();
            return services;
        }
    }
}

