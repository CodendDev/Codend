using FluentResults;

namespace Codend.Domain.Entities.ProjectTask.Abstractions;

/// <summary>
/// Interface for which allows creating a ProjectTask. 
/// ProjectTask which implements this interface can be created using <see cref="IProjectTaskCreator{TProjectTask,TProps}.Create"/> method.
/// </summary>
/// <typeparam name="TProjectTask">ProjectTask class.</typeparam>
/// <typeparam name="TProps">Properties which will be used for creation.</typeparam>
public interface IProjectTaskCreator<TProjectTask, in TProps>
    where TProjectTask : BaseProjectTask
    where TProps : IProjectTaskCreateProperties
{
    /// <summary>
    /// Creation method for <see cref="TProjectTask"/>.
    /// </summary>
    /// <param name="properties">Properties which will be used for creation.</param>
    /// <param name="ownerId">UserId of the task owner.</param>
    /// <returns></returns>
    static abstract Result<TProjectTask> Create(TProps properties, UserId ownerId);
}