using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Contracts.Automapper.Entities.ProjectTask;

/// <summary>
/// <see cref="BaseProjectTask"/> and <see cref="BugfixProjectTask"/> board automapper profiles.
/// </summary>
public sealed class BoardProjectTaskResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for all task types to <see cref="BoardProjectTaskResponse"/> class.
    /// </summary>
    public BoardProjectTaskResponseProfile()
    {
        CreateMap<BaseProjectTask, BoardProjectTaskResponse>();
    }
}