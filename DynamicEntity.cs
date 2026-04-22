using System;
using System.Collections.Concurrent;
using SampSharp.Entities;

namespace SampSharp.Streamer.Entities;

/// <summary>
/// Base class for all streamer-managed entities (objects, pickups, text labels,
/// map icons, checkpoints). Unlike ECS <see cref="SampSharp.Entities.Component"/>,
/// a <see cref="DynamicEntity"/> lives inside streamer.dll's memory rather than
/// the EntityManager: identity is the streamer-id alone, lifetime is controlled by
/// explicit Destroy* calls.
/// </summary>
/// <remarks>
/// <see cref="EventParameterAttribute"/> tells <see cref="EventDispatcher"/> to
/// pass derived instances straight through as event arguments (the same way it
/// treats value types and arrays) instead of trying to resolve them from DI.
/// Without it the dispatcher would think an [Event] handler takes fewer
/// arguments than we actually pass — for example, OnPlayerPickUpDynamicPickup
/// dispatches (EntityId, DynamicPickup) but a DI-classified parameter would be
/// counted as zero, giving the "argument count mismatch" log and silently
/// skipping the handler.
/// </remarks>
[EventParameter]
public abstract class DynamicEntity
{
    /// <summary>
    /// Per-type registry mapping streamer-id → stable <see cref="EntityId"/>.
    /// Required so that the same streamer item — whether obtained from a
    /// Create* call or from a streamer event — yields the same EntityId,
    /// because legacy VSRP code keys Dictionary&lt;EntityId, …&gt; on this.
    /// </summary>
    private static readonly ConcurrentDictionary<(System.Type, int), EntityId> _entityRegistry = new();

    private EntityId _entity;

    /// <summary>Streamer-side id. Stable for the lifetime of the entity.</summary>
    public int Id { get; protected set; }

    /// <summary>
    /// Stable EntityId, identical for every managed wrapper of the same
    /// streamer item (obtained either via Create* or via a streamer event).
    /// </summary>
    public EntityId Entity
    {
        get
        {
            if (_entity == default)
                _entity = _entityRegistry.GetOrAdd((GetType(), Id), _ => EntityId.NewEntityId());
            return _entity;
        }
    }

    public abstract bool IsAlive { get; }
    public abstract void DestroyEntity();

    /// <summary>Legacy alias mirroring the old SampSharp Component.IsComponentAlive.</summary>
    public bool IsComponentAlive => IsAlive;

    /// <summary>Legacy alias for pattern matching like <c>is { IsValid: true }</c>.</summary>
    public bool IsValid => IsAlive;

    /// <summary>Legacy alias for Component.Destroy().</summary>
    public void Destroy() => DestroyEntity();

    /// <summary>
    /// Drops the cached EntityId from the registry. DestroyEntity implementations
    /// should call this so that re-creating a streamer item with the same id
    /// produces a fresh Guid instead of resurrecting a dead one.
    /// </summary>
    protected void ForgetEntity()
    {
        _entityRegistry.TryRemove((GetType(), Id), out _);
        _entity = default;
    }

    public override string ToString() => $"{GetType().Name}(Id={Id})";

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not DynamicEntity other) return false;
        return GetType() == other.GetType() && Id == other.Id;
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(DynamicEntity? left, DynamicEntity? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(DynamicEntity? left, DynamicEntity? right) => !(left == right);
}
