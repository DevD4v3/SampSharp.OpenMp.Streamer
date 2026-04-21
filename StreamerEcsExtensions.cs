using SampSharp.Entities;

namespace SampSharp.Streamer.Entities;

public static class StreamerEcsExtensions
{
    /// <summary>
    /// Регистрирует streamer events в ECS builder'е. Сейчас — no-op (phase 1: только API).
    /// Phase 2: регистрация OnPlayerPickUpDynamicPickup / OnPlayerEnterDynamicCP /
    /// OnPlayerSelectDynamicObject / OnPlayerEditDynamicObject и пр.
    /// </summary>
    public static IEcsBuilder EnableStreamerEvents(this IEcsBuilder builder) => builder;
}
