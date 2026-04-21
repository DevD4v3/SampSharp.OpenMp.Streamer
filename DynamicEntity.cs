namespace SampSharp.Streamer.Entities;

/// <summary>
/// Базовый класс для всех стример-сущностей (объекты, пикапы, текстовые метки, иконки
/// карты, checkpoint'ы). В отличие от ECS <see cref="SampSharp.Entities.Component"/>,
/// <see cref="DynamicEntity"/> живёт в памяти streamer.dll, а не в EntityManager:
/// уникальность обеспечивается только streamer-id, время жизни — явным Destroy*.
///
/// Свойство <see cref="IsAlive"/> переходит в false после <see cref="DestroyEntity"/>,
/// но сам объект не выкидывается из managed-heap — его можно отбросить по GC.
/// </summary>
public abstract class DynamicEntity
{
    /// <summary>Внутренний streamer ID. Стабилен на всё время жизни сущности.</summary>
    public int Id { get; protected set; }

    /// <summary>
    /// Отображает, жив ли объект в streamer.dll. Может вернуть false если сущность
    /// была уничтожена через Destroy* или если streamer не был загружен.
    /// </summary>
    public abstract bool IsAlive { get; }

    /// <summary>Уничтожает сущность в streamer.dll. Идемпотентен.</summary>
    public abstract void DestroyEntity();

    /// <summary>Legacy-alias: в старом SampSharp ECS у Component был <c>IsComponentAlive</c>.</summary>
    public bool IsComponentAlive => IsAlive;

    /// <summary>Legacy-alias для pattern matching <c>is {IsValid: true}</c>.</summary>
    public bool IsValid => IsAlive;

    /// <summary>Legacy alias: в старом ECS у Component был метод Destroy().</summary>
    public void Destroy() => DestroyEntity();

    /// <summary>
    /// Legacy: Component.Entity возвращает <see cref="SampSharp.Entities.EntityId"/>.
    /// Streamer-items не имеют ECS-entity, но legacy-код иногда читает это свойство.
    /// Возвращаем default (Empty) EntityId — вызовы типа player.GetComponent(dyn.Entity)
    /// не будут находить ничего, но компиляция проходит.
    /// </summary>
    public SampSharp.Entities.EntityId Entity => default;

    public override string ToString() => $"{GetType().Name}(Id={Id})";
}
