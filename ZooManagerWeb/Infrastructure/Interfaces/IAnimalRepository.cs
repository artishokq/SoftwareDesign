namespace ZooManagerWeb.Infrastructure.Interfaces;

using ZooManagerWeb.Domain.Entities;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(Guid id);
    Task<IEnumerable<Animal>> GetAllAsync();
    Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId);
    Task AddAsync(Animal animal);
    Task UpdateAsync(Animal animal);
    Task DeleteAsync(Guid id);
}