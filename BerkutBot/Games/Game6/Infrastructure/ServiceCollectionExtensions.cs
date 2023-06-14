using System;
using BerkutBot.Games.Game6.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game6.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame6Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game6IncorrectCommandHandler>();
            services.AddTransient<IGameAnswer, Game6StartCommandHandler>();
            services.AddTransient<IGameAnswer, Game6HelpCommandHandler>();
            //services.AddTransient<IGameAnswer, Game6AnswerGo>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();
            return services;
        }
    }
}

