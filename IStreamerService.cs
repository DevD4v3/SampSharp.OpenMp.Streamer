using System.Numerics;
using SampSharp.Entities;
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
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 300f, float drawDistance = 0f,
        EntityId area = default, int priority = 0);

    /// <summary>Legacy-перегрузка: 11 аргументов, где последний — "parent" entity (игнорируется MVP).</summary>
    DynamicObject CreateDynamicObject(int modelId, Vector3 position, Vector3 rotation,
        int virtualWorld, int interior, EntityId player,
        float streamDistance, float drawDistance,
        EntityId area, int priority, EntityId parent);

    /// <summary>Legacy: areaId как int (PAWN-native style).</summary>
    DynamicObject CreateDynamicObject(int modelId, Vector3 position, Vector3 rotation,
        int virtualWorld, int interior, EntityId player,
        float streamDistance, float drawDistance,
        int areaId, int priority, EntityId parent = default);

    // Pickups
    DynamicPickup CreateDynamicPickup(int modelId, PickupType type, Vector3 position,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, EntityId area = default, int priority = 0);

    // 3D Text Labels
    DynamicTextLabel CreateDynamicTextLabel(string text, Color color, Vector3 position,
        float drawDistance,
        EntityId attachedPlayer = default, EntityId attachedVehicle = default,
        bool testLos = false,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, EntityId area = default, int priority = 0);

    // Map Icons
    DynamicMapIcon CreateDynamicMapIcon(Vector3 position, MapIcon type, Color color,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, MapIconType style = MapIconType.Local,
        EntityId area = default, int priority = 0);

    // Checkpoints
    DynamicCheckpoint CreateDynamicCheckpoint(Vector3 position, float size,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, EntityId area = default, int priority = 0);
}
