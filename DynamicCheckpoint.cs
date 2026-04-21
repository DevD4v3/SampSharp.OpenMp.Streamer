using System.Numerics;
using SampSharp.Streamer.Entities.Interop;

namespace SampSharp.Streamer.Entities;

/// <summary>Streamer-managed checkpoint (PAWN-native: CreateDynamicCP).</summary>
public sealed class DynamicCheckpoint : DynamicEntity
{
    internal DynamicCheckpoint(int id, Vector3 position, float size)
    {
        Id = id;
        Position = position;
        Size = size;
    }

    public Vector3 Position { get; }
    public float Size { get; }

    public override bool IsAlive => StreamerInterop.Streamer_Checkpoint_IsValid(Id);
    public override void DestroyEntity() => StreamerInterop.Streamer_Checkpoint_Destroy(Id);
}
