using HSE_BANK.DataAccess;
using HSE_BANK.Domain;

namespace HSE_BANK.Facades;

public class CategoryFacade
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryFacade(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Category CreateCategory(CategoryType type, string name)
    {
        var category = DomainFactory.CreateCategory(type, name);
        _categoryRepository.Add(category);
        return category;
    }

    public void EditCategory(Guid id, string newName)
    {
        var category = _categoryRepository.GetById(id);
        if (category == null)
        {
            throw new Exception("Категория не найдена");
        }

        _categoryRepository.Delete(id);
        var updatedCategory = new Category(category.Type, newName);
        _categoryRepository.Add(updatedCategory);
    }

    public void DeleteCategory(Guid id)
    {
        _categoryRepository.Delete(id);
    }
}