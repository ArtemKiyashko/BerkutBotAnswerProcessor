using BerkutBot.Games.Game13.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game13.Infrastructure;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddGame13Services(this IServiceCollection services)
    {
        services.AddTransient<IGameAnswer, Game13AnswerGo>();
        services.AddTransient<IGameAnswer, Game13AnswerChaika>();
        services.AddTransient<IGameAnswer, Game13AnswerGora>();
        services.AddTransient<IGameAnswer, Game13AnswerWhiteRose>();

        services.AddTransient<IStartCommand, TestPoint>();
        services.AddTransient<IStartCommand, Point1>();
        services.AddTransient<IStartCommand, Point3>();
        services.AddTransient<IStartCommand, Point5>();
        services.AddTransient<IStartCommand, Point6>();
        services.AddTransient<IStartCommand, Point7>();
        services.AddTransient<IStartCommand, Point8>();
        return services;
    }
}
