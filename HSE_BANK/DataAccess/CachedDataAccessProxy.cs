using System.Collections.Generic;
using System.Linq;

namespace HSE_BANK.DataAccess;

public class CachedDataAccessProxy<T> : IRepository<T> where T : class
{
    private readonly IRepository<T> _innerRepository;
    private List<T> _cache;

    public CachedDataAccessProxy(IRepository<T> innerRepository)
    {
        _innerRepository = innerRepository;
        _cache = _innerRepository.GetAll().ToList();
    }

    public void Add(T entity)
    {
        _innerRepository.Add(entity);
        _cache.Add(entity);
    }

    public void Delete(Guid id)
    {
        _innerRepository.Delete(id);
        _cache = _cache.Where(e => (Guid)e.GetType().GetProperty("Id").GetValue(e) != id).ToList();
    }

    public IEnumerable<T> GetAll()
    {
        return _cache;
    }

    public T GetById(Guid id)
    {
        return _cache.FirstOrDefault(e => (Guid)e.GetType().GetProperty("Id").GetValue(e) == id);
    }

    public void Update(T entity)
    {
        _innerRepository.Update(entity);
        var id = (Guid)entity.GetType().GetProperty("Id").GetValue(entity);
        var index = _cache.FindIndex(e => (Guid)e.GetType().GetProperty("Id").GetValue(e) == id);
        if (index != -1)
        {
            _cache[index] = entity;
        }
    }
}