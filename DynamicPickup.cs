using System.Numerics;
using SampSharp.Entities.SAMP;
using SampSharp.Streamer.Entities.Interop;

namespace SampSharp.Streamer.Entities;

/// <summary>Streamer-managed pickup (PAWN-native: CreateDynamicPickup).</summary>
public sealed class DynamicPickup : DynamicEntity
{
    internal DynamicPickup(int id, int modelId, PickupType type, Vector3 position)
    {
        Id = id;
        ModelId = modelId;
        Type = type;
        Position = position;
    }

    public int ModelId { get; }
    public PickupType Type { get; }
    public Vector3 Position { get; }

    public override bool IsAlive => StreamerInterop.Streamer_Pickup_IsValid(Id);
    public override void DestroyEntity() => StreamerInterop.Streamer_Pickup_Destroy(Id);
}
