using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Messaging.Commands;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentResults;
using ProjectTaskNotFound = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskErrors.ProjectTaskNotFound;

namespace Codend.Application.ProjectTasks.Commands.DeleteProjectTask;

public sealed record DeleteProjectTaskCommand(Guid ProjectTaskId) : ICommand;

public class DeleteProjectTaskCommandHandler : ICommandHandler<DeleteProjectTaskCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectTaskCommandHandler(IProjectTaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await _taskRepository.GetByIdAsync(new ProjectTaskId(request.ProjectTaskId));
        if (project is null)
        {
            return Result.Fail(new ProjectTaskNotFound());
        }

        _taskRepository.Remove(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}