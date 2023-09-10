namespace Codend.Domain.Entities;

/// <summary>
/// ProjectTask has to implement this interface to be persisted in database.
/// </summary>
/// <typeparam name="TProjectTask"><see cref="AbstractProjectTask"/> derived class.</typeparam>
/// <typeparam name="TCreateProperties">
/// Properties class which will be used for creation.
/// Properties must implement <see cref="ProjectTaskProperties"/>
/// </typeparam>
/// <typeparam name="TUpdateProperties">
/// Properties class which will be used for update.
/// Properties must implement <see cref="UpdateAbstractProjectTaskProperties"/>
/// </typeparam>
public interface IPersistentProjectTask<TProjectTask, in TCreateProperties, in TUpdateProperties> :
    IProjectTaskCreator<TProjectTask, TCreateProperties>,
    IProjectTaskUpdater<TProjectTask, TUpdateProperties>
    where TProjectTask : AbstractProjectTask
    where TCreateProperties : ProjectTaskProperties
    where TUpdateProperties : UpdateAbstractProjectTaskProperties

{
}