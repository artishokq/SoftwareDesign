namespace ZooManagerWeb.Infrastructure.Repositories;

using ZooManagerWeb.Domain.Entities;
using ZooManagerWeb.Infrastructure.Interfaces;

public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly List<Enclosure> _enclosures = new List<Enclosure>();

    public Task<Enclosure?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_enclosures.FirstOrDefault(e => e.Id == id));
    }

    public Task<IEnumerable<Enclosure>> GetAllAsync()
    {
        return Task.FromResult(_enclosures.AsEnumerable());
    }

    public Task AddAsync(Enclosure enclosure)
    {
        _enclosures.Add(enclosure);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Enclosure enclosure)
    {
        var existingEnclosure = _enclosures.FirstOrDefault(e => e.Id == enclosure.Id);
        if (existingEnclosure != null)
        {
            _enclosures.Remove(existingEnclosure);
            _enclosures.Add(enclosure);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var enclosure = _enclosures.FirstOrDefault(e => e.Id == id);
        if (enclosure != null)
        {
            _enclosures.Remove(enclosure);
        }
        return Task.CompletedTask;
    }
}