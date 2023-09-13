using Codend.Domain.Entities;

namespace Codend.Domain.Repositories;

public interface IStoryRepository
{
    void Add(Story story);
}