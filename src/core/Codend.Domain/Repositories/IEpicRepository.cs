using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IEpicRepository
{
    void Add(Epic epic);
    Task<Epic?> GetByIdAsync(EpicId epicId);
    void Update(Epic epic);
    void Remove(Epic epic);
    Task<List<Epic>> GetEpicsWithStatusId(ProjectTaskStatusId statusId);
}