using BerkutBot.Games.Game11.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game11.Infrastructure;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddGame11Services(this IServiceCollection services)
    {
        services.AddTransient<IStartCommand, TestPoint>();
        services.AddTransient<IStartCommand, ArQr>();
        services.AddTransient<IStartCommand, Point1>();
        services.AddTransient<IStartCommand, Point4>();
        services.AddTransient<IStartCommand, Point5>();
        services.AddTransient<IStartCommand, Point6>();
        services.AddTransient<IStartCommand, Point8>();

        services.AddTransient<IGameAnswer, Game11Answer12024>();
        services.AddTransient<IGameAnswer, Game11AnswerColdWater>();
        services.AddTransient<IGameAnswer, Game11AnswerTestMenuOff>();
        services.AddTransient<IGameAnswer, Game11AnswerTestMenuOn>();
        return services;
    }
}
