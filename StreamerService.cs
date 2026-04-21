using System;
using System.Numerics;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.Streamer.Entities.Interop;

namespace SampSharp.Streamer.Entities;

/// <summary>
/// Реализация <see cref="IStreamerService"/>, пробрасывающая вызовы в streamer.dll
/// через C-exports в SampSharp.dll (<see cref="StreamerInterop"/>).
/// </summary>
public sealed class StreamerService : IStreamerService
{
    private const int InvalidId = 0xFFFF;

    public bool IsAvailable => StreamerInterop.Streamer_IsAvailable();

    public DynamicObject CreateDynamicObject(int modelId, Vector3 position, Vector3 rotation,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 300f, float drawDistance = 0f,
        EntityId area = default, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Object_Create(modelId,
            position.X, position.Y, position.Z,
            rotation.X, rotation.Y, rotation.Z,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            streamDistance, drawDistance, EntityToInt(area, -1), priority);
        return new DynamicObject(id, modelId, position, rotation);
    }

    public DynamicObject CreateDynamicObject(int modelId, Vector3 position, Vector3 rotation,
        int virtualWorld, int interior, EntityId player,
        float streamDistance, float drawDistance,
        EntityId area, int priority, EntityId parent)
    {
        var obj = CreateDynamicObject(modelId, position, rotation,
            virtualWorld, interior, player, streamDistance, drawDistance, area, priority);
        if (!parent.IsEmpty) obj.AttachTo(parent, Vector3.Zero, Vector3.Zero);
        return obj;
    }

    public DynamicObject CreateDynamicObject(int modelId, Vector3 position, Vector3 rotation,
        int virtualWorld, int interior, EntityId player,
        float streamDistance, float drawDistance,
        int areaId, int priority, EntityId parent = default)
    {
        int id = StreamerInterop.Streamer_Object_Create(modelId,
            position.X, position.Y, position.Z,
            rotation.X, rotation.Y, rotation.Z,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            streamDistance, drawDistance, areaId, priority);
        var obj = new DynamicObject(id, modelId, position, rotation);
        if (!parent.IsEmpty) obj.AttachTo(parent, Vector3.Zero, Vector3.Zero);
        return obj;
    }

    public DynamicPickup CreateDynamicPickup(int modelId, PickupType type, Vector3 position,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, EntityId area = default, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Pickup_Create(modelId, (int)type,
            position.X, position.Y, position.Z,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            streamDistance, EntityToInt(area, -1), priority);
        return new DynamicPickup(id, modelId, type, position);
    }

    public DynamicTextLabel CreateDynamicTextLabel(string text, Color color, Vector3 position,
        float drawDistance,
        EntityId attachedPlayer = default, EntityId attachedVehicle = default,
        bool testLos = false,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, EntityId area = default, int priority = 0)
    {
        uint argb = ToArgb(color);
        int id = StreamerInterop.Streamer_TextLabel_Create(text ?? string.Empty, argb,
            position.X, position.Y, position.Z, drawDistance,
            EntityToInt(attachedPlayer, InvalidId), EntityToInt(attachedVehicle, InvalidId),
            testLos,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            streamDistance, EntityToInt(area, -1), priority);
        return new DynamicTextLabel(id, text ?? string.Empty, color, position, drawDistance);
    }

    public DynamicMapIcon CreateDynamicMapIcon(Vector3 position, MapIcon type, Color color,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, MapIconType style = MapIconType.Local,
        EntityId area = default, int priority = 0)
    {
        uint argb = ToArgb(color);
        int id = StreamerInterop.Streamer_MapIcon_Create(
            position.X, position.Y, position.Z, (int)type, argb,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            streamDistance, (int)style, EntityToInt(area, -1), priority);
        return new DynamicMapIcon(id, position, type, color);
    }

    public DynamicCheckpoint CreateDynamicCheckpoint(Vector3 position, float size,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        float streamDistance = 200f, EntityId area = default, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Checkpoint_Create(
            position.X, position.Y, position.Z, size,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            streamDistance, EntityToInt(area, -1), priority);
        return new DynamicCheckpoint(id, position, size);
    }

    public DynamicActor CreateDynamicActor(int modelId, Vector3 position, float rotation,
        bool invulnerable = true, float health = 100f,
        float streamDistance = 300f,
        int virtualWorld = -1, int interior = -1, EntityId player = default,
        EntityId area = default, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Actor_Create(modelId,
            position.X, position.Y, position.Z, rotation,
            invulnerable, health, streamDistance,
            virtualWorld, interior, EntityToInt(player, InvalidId),
            EntityToInt(area, -1), priority);
        return new DynamicActor(id, modelId, position, rotation);
    }

    // EntityId хэширует Guid → стабильный int ≠ playerid. Для streamer-а filter-id
    // берутся игровые (0..MAX_PLAYERS-1); пока proxy: пустой EntityId → "no filter".
    private static int EntityToInt(EntityId id, int defaultValue)
        => id.IsEmpty ? defaultValue : id.Handle;

    private static uint ToArgb(Color c) => ((uint)c.A << 24) | ((uint)c.R << 16) | ((uint)c.G << 8) | c.B;
}
