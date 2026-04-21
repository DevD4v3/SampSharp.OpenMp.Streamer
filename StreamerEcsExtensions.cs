using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.OpenMp.Core;

namespace SampSharp.Streamer.Entities;

public static class StreamerEcsExtensions
{
    /// <summary>
    /// Регистрирует streamer events в ECS builder'е. В Phase 2 это активирует
    /// <see cref="StreamerEventSystem"/>, который при старте attach'ит
    /// UnmanagedCallersOnly callbacks в SampSharp.dll → streamer.dll.
    /// </summary>
    public static IEcsBuilder EnableStreamerEvents(this IEcsBuilder builder) => builder;

    /// <summary>
    /// Зарегистрировать в DI все сервисы streamer'а. Вызвать в ConfigureServices до
    /// AddSystemsInAssembly. Порядок: сначала IStreamerService, затем event-система.
    /// </summary>
    public static IServiceCollection AddStreamer(this IServiceCollection services)
    {
        services.AddSingleton<IStreamerService, StreamerService>();
        services.AddSystem<StreamerEventSystem>();
        return services;
    }
}
