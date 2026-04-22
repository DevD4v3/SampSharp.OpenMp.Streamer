using System.Numerics;
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
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 300f, float drawDistance = 0f,
        int areaId = -1, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Object_Create(modelId,
            position.X, position.Y, position.Z,
            rotation.X, rotation.Y, rotation.Z,
            virtualWorld, interior, PlayerToId(player),
            streamDistance, drawDistance, areaId, priority);
        return new DynamicObject(id, modelId, position, rotation);
    }

    public DynamicPickup CreateDynamicPickup(int modelId, PickupType type, Vector3 position,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, int areaId = -1, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Pickup_Create(modelId, (int)type,
            position.X, position.Y, position.Z,
            virtualWorld, interior, PlayerToId(player),
            streamDistance, areaId, priority);
        return new DynamicPickup(id, modelId, type, position);
    }

    public DynamicTextLabel CreateDynamicTextLabel(string text, Color color, Vector3 position,
        float drawDistance,
        Player? attachedPlayer = null, Vehicle? attachedVehicle = null,
        bool testLos = false,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, int areaId = -1, int priority = 0)
    {
        uint argb = ToArgb(color);
        int id = StreamerInterop.Streamer_TextLabel_Create(text ?? string.Empty, argb,
            position.X, position.Y, position.Z, drawDistance,
            PlayerToId(attachedPlayer), VehicleToId(attachedVehicle),
            testLos,
            virtualWorld, interior, PlayerToId(player),
            streamDistance, areaId, priority);
        return new DynamicTextLabel(id, text ?? string.Empty, color, position, drawDistance);
    }

    public DynamicMapIcon CreateDynamicMapIcon(Vector3 position, MapIcon type, Color color,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, MapIconType style = MapIconType.Local,
        int areaId = -1, int priority = 0)
    {
        uint rgba = ToRgba(color);
        int id = StreamerInterop.Streamer_MapIcon_Create(
            position.X, position.Y, position.Z, (int)type, rgba,
            virtualWorld, interior, PlayerToId(player),
            streamDistance, (int)style, areaId, priority);
        return new DynamicMapIcon(id, position, type, color);
    }

    public DynamicCheckpoint CreateDynamicCheckpoint(Vector3 position, float size,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        float streamDistance = 200f, int areaId = -1, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Checkpoint_Create(
            position.X, position.Y, position.Z, size,
            virtualWorld, interior, PlayerToId(player),
            streamDistance, areaId, priority);
        return new DynamicCheckpoint(id, position, size);
    }

    public DynamicActor CreateDynamicActor(int modelId, Vector3 position, float rotation,
        bool invulnerable = true, float health = 100f,
        float streamDistance = 300f,
        int virtualWorld = -1, int interior = -1, Player? player = null,
        int areaId = -1, int priority = 0)
    {
        int id = StreamerInterop.Streamer_Actor_Create(modelId,
            position.X, position.Y, position.Z, rotation,
            invulnerable, health, streamDistance,
            virtualWorld, interior, PlayerToId(player),
            areaId, priority);
        return new DynamicActor(id, modelId, position, rotation);
    }

    private static int PlayerToId(Player? player) => player is { IsComponentAlive: true } ? player.Id : InvalidId;
    private static int VehicleToId(Vehicle? vehicle) => vehicle is { IsComponentAlive: true } ? vehicle.Id : InvalidId;

    private static uint ToArgb(Color c) => ((uint)c.A << 24) | ((uint)c.R << 16) | ((uint)c.G << 8) | c.B;
    private static uint ToRgba(Color c) => ((uint)c.R << 24) | ((uint)c.G << 16) | ((uint)c.B << 8) | c.A;
}
