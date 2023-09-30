using System;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGameCommonServices(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, GameIncorrectCommandHandler>();
            services.AddTransient<IGameAnswer, GameStartCommandHandler>();
            services.AddTransient<IGameAnswer, GameHelpCommandHandler>();
            services.AddTransient<IStartCommand, GameDefaultStartCommand>();
            services.AddTransient<IStartCommand, GameUnknownStartCommand>();
            return services;
        }
    }
}

