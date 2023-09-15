using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IEpicRepository
{
    void Add(Epic epic);
    Task<Epic?> GetByIdAsync(EpicId storyId);
    void Update(Epic story);
}