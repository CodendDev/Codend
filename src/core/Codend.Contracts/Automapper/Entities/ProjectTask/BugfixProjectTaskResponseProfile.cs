using AutoMapper;
using Codend.Contracts.Responses.ProjectTask;
using Codend.Domain.Entities.ProjectTask.Bugfix;

namespace Codend.Contracts.Automapper.Entities.ProjectTask;

public sealed class BugfixProjectTaskResponseProfile : Profile
{
    public BugfixProjectTaskResponseProfile()
    {
        CreateMap<BugfixProjectTask, BugfixProjectTaskResponse>();
    }
}