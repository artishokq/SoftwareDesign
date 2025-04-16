namespace ZooManagerWeb.Infrastructure.Repositories;

using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Interfaces;

public class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _animals = new List<Animal>();

    public Task<Animal?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_animals.FirstOrDefault(a => a.Id == id));
    }

    public Task<IEnumerable<Animal>> GetAllAsync()
    {
        return Task.FromResult(_animals.AsEnumerable());
    }

    public Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId)
    {
        var result = _animals.Where(a => a.CurrentEnclosureId == enclosureId);
        return Task.FromResult(result);
    }

    public Task AddAsync(Animal animal)
    {
        _animals.Add(animal);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Animal animal)
    {
        var existingAnimal = _animals.FirstOrDefault(a => a.Id == animal.Id);
        if (existingAnimal != null)
        {
            _animals.Remove(existingAnimal);
            _animals.Add(animal);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal != null)
        {
            _animals.Remove(animal);
        }
        return Task.CompletedTask;
    }
}