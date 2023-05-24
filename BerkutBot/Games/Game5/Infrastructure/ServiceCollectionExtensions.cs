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
            services.AddTransient<IGameAnswer, Game5HelpCommandHandler>();
            services.AddTransient<IGameAnswer, Game5AnswerGo>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();
            services.AddTransient<IStartCommand, Point1>();
            services.AddTransient<IStartCommand, Point2>();
            services.AddTransient<IStartCommand, Point3>();
            services.AddTransient<IStartCommand, Point4>();
            services.AddTransient<IStartCommand, Point5>();
            services.AddTransient<IStartCommand, Point6>();
            return services;
        }
    }
}

