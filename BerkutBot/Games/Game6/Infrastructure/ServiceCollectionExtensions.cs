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
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();

            services.AddTransient<IStartCommand, Point1>();
            services.AddTransient<IStartCommand, Point2>();
            services.AddTransient<IStartCommand, Point3>();
            services.AddTransient<IStartCommand, Point4>();
            services.AddTransient<IStartCommand, Point5>();
            services.AddTransient<IStartCommand, Point6>();
            services.AddTransient<IStartCommand, Point7>();
            services.AddTransient<IStartCommand, Point9>();
            services.AddTransient<IStartCommand, Point10>();
            services.AddTransient<IStartCommand, Point11>();
            services.AddTransient<IStartCommand, Point12>();

            services.AddTransient<IGameAnswer, Game6Answer300>();
            return services;
        }
    }
}

