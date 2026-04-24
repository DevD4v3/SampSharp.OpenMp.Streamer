using System.Numerics;
using SampSharp.Entities.SAMP;

namespace SampSharp.Streamer.Entities;

/// <summary>
/// Клиентский API к streamer.dll. Реализация живёт в <see cref="StreamerService"/>,
/// которая вызывает C-exports в SampSharp.dll → IStreamerComponent extension в streamer.dll.
///
/// Если streamer.dll не загружен сервером, все методы возвращают "мёртвые" сущности
/// с <c>Id=0</c>, <c>IsAlive=false</c>. Проверить наличие сервиса можно через
/// <see cref="IsAvailable"/>.
/// </summary>
public interface IStreamerService
{
    /// <summary>Подгружен ли streamer.dll и видит ли его SampSharp.dll extension-слой.</summary>
    bool IsAvailable { get; }

    // Objects
    DynamicObject CreateDynamicObject(int modelId, Vector3 position, Vector3 rotation,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 300f, float drawDistance = 0f,
        int areaId = -1, int priority = 0);

    // Pickups
    DynamicPickup CreateDynamicPickup(int modelId, PickupType type, Vector3 position,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, int areaId = -1, int priority = 0);

    // 3D Text Labels
    DynamicTextLabel CreateDynamicTextLabel(string text, Color color, Vector3 position,
        float drawDistance,
        Player? attachedPlayer = null, Vehicle? attachedVehicle = null,
        bool testLos = false,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, int areaId = -1, int priority = 0);

    // Map Icons
    DynamicMapIcon CreateDynamicMapIcon(Vector3 position, MapIcon type, Color color,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, MapIconType style = MapIconType.Local,
        int areaId = -1, int priority = 0);

    // Checkpoints
    DynamicCheckpoint CreateDynamicCheckpoint(Vector3 position, float size,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, int areaId = -1, int priority = 0);

    // Actors
    DynamicActor CreateDynamicActor(int modelId, Vector3 position, float rotation,
        bool invulnerable = true, float health = 100f,
        float streamDistance = 300f,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        int areaId = -1, int priority = 0);

    // ========================================================================
    // Telemetry (VS:RP fork) — cumulative since last ResetPhaseStats().
    // Call ResetPhaseStats() every ~1 minute for rolling-window stats.
    // ========================================================================

    /// <summary>Snapshot of timing + stream counts for one streamer type.</summary>
    StreamerPhaseStats GetPhaseStats(StreamerType type);

    /// <summary>Total per-player discovery ticks recorded since the last reset.</summary>
    ulong GetPhaseTickCount();

    /// <summary>Zero every telemetry counter.</summary>
    void ResetPhaseStats();

    // ========================================================================
    // Anti-flicker hysteresis (VS:RP fork). When a player sits on the boundary
    // of an item's stream distance, items ping-pong in/out every tick. With
    // factor > 1.0 the stream-out check is delayed until the player is
    // factor × streamDistance away. Applies to Object / MapIcon / TextLabel;
    // other types ignore the setting.
    // ========================================================================

    float GetHysteresisFactor(StreamerType type);
    /// <summary>Valid range [1.0, 10.0]. Returns false on invalid input.</summary>
    bool SetHysteresisFactor(StreamerType type, float factor);

    // ========================================================================
    // Two-tier grid (VS:RP fork). Items with streamDistance between
    // cellDistance and CoarseCellDistance bucket into larger coarse cells
    // instead of the O(N) globalCell catch-all. CoarseCellDistance = 0
    // disables the tier. Setters rebuild the grid synchronously.
    // ========================================================================

    float CoarseCellSize { get; }
    bool TrySetCoarseCellSize(float size);
    float CoarseCellDistance { get; }
    bool TrySetCoarseCellDistance(float distance);
}
