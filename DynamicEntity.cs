using System.Collections.Concurrent;
using SampSharp.Entities;

namespace SampSharp.Streamer.Entities;

/// <summary>
/// Базовый класс для всех стример-сущностей (объекты, пикапы, текстовые метки, иконки
/// карты, checkpoint'ы). В отличие от ECS <see cref="SampSharp.Entities.Component"/>,
/// <see cref="DynamicEntity"/> живёт в памяти streamer.dll, а не в EntityManager:
/// уникальность обеспечивается только streamer-id, время жизни — явным Destroy*.
/// </summary>
public abstract class DynamicEntity
{
    /// <summary>
    /// Per-type registry: int streamer-id → EntityId (стабильный Guid).
    /// Нужен, чтобы при create и при event для одного и того же streamer-item
    /// <c>entity.Entity</c> был одним и тем же EntityId — legacy VSRP-код использует
    /// это как ключ в Dictionary.
    /// </summary>
    private static readonly ConcurrentDictionary<(System.Type, int), EntityId> _entityRegistry = new();

    private EntityId _entity;

    /// <summary>Внутренний streamer ID. Стабилен на всё время жизни сущности.</summary>
    public int Id { get; protected set; }

    /// <summary>
    /// Стабильный EntityId, один и тот же для всех managed-обёрток одного и того же
    /// streamer-item'а (через <c>Create*</c> или через event).
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

    /// <summary>Legacy-alias: старый SampSharp Component.IsComponentAlive.</summary>
    public bool IsComponentAlive => IsAlive;

    /// <summary>Legacy-alias для pattern matching <c>is {IsValid: true}</c>.</summary>
    public bool IsValid => IsAlive;

    /// <summary>Legacy alias: Component.Destroy().</summary>
    public void Destroy() => DestroyEntity();

    /// <summary>
    /// Выкидывает кешированный EntityId из registry. Вызывают реализации
    /// <see cref="DestroyEntity"/>, чтобы пересоздание sh-item'а с тем же
    /// streamer-id получало свежий Guid.
    /// </summary>
    protected void ForgetEntity()
    {
        _entityRegistry.TryRemove((GetType(), Id), out _);
        _entity = default;
    }

    public override string ToString() => $"{GetType().Name}(Id={Id})";
}
