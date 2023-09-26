using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Contracts.Automapper.Entities.ProjectTask;

/// <summary>
/// <see cref="BugfixProjectTask"/> automapper profile.
/// </summary>
public sealed class BugfixProjectTaskResponseProfile : Profile
{
    /// <summary>
    /// Initializes maps for <see cref="BugfixProjectTask"/> class.
    /// </summary>
    public BugfixProjectTaskResponseProfile()
    {
        CreateMap<BugfixProjectTask, BugfixProjectTaskResponse>();
    }
}