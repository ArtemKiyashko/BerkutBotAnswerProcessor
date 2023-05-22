using System;
using BerkutBot.Games.Game5.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game5.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame5Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game5IncorrectCommandHandler>();
            services.AddTransient<IGameAnswer, Game5StartCommandHandler>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();
            services.AddTransient<IStartCommand, Point1>();
            services.AddTransient<IStartCommand, Point2>();
            return services;
        }
    }
}

