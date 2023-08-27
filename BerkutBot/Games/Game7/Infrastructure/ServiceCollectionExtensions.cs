using BerkutBot.Games.Game7.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game7.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame7Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game7IncorrectCommandHandler>();
            services.AddTransient<IGameAnswer, Game7StartCommandHandler>();
            services.AddTransient<IGameAnswer, Game7HelpCommandHandler>();
            services.AddTransient<IGameAnswer, Game7AnswerGo>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();

            services.AddTransient<IStartCommand, Point1>();
            services.AddTransient<IStartCommand, Point2>();
            services.AddTransient<IStartCommand, Point3>();
            services.AddTransient<IStartCommand, Point4>();
            services.AddTransient<IStartCommand, Point5>();
            services.AddTransient<IStartCommand, Point6>();
            services.AddTransient<IStartCommand, TestPoint>();

            return services;
        }
    }
}

