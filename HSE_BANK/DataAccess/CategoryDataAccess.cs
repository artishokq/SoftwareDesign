using HSE_BANK.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HSE_BANK.DataAccess;

public class CategoryDataAccess : IRepository<Category>
{
    private readonly List<Category> _categories = new List<Category>();

    public void Add(Category entity)
    {
        _categories.Add(entity);
    }

    public void Delete(Guid id)
    {
        var category = GetById(id);
        if (category != null)
        {
            _categories.Remove(category);
        }
    }

    public IEnumerable<Category> GetAll()
    {
        return _categories;
    }

    public Category GetById(Guid id)
    {
        return _categories.FirstOrDefault(c => c.Id == id);
    }

    public void Update(Category entity)
    {
        // В данном примере класс Category является иммутабельным, поэтому обновление
        // выполняется через удаление и создание нового объекта
    }
}