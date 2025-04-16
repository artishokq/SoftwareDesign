namespace ZooManagerWeb.Domain.Entities;

using ZooManagerWeb.Domain.Value_Object.Enclosure;

public class Enclosure
{
    public Guid Id { get; }
    public EnclosureType Type { get; }
    public int Size { get; }
    private readonly List<Guid> _animalIds;
    public IReadOnlyCollection<Guid> AnimalIds => _animalIds.AsReadOnly();
    public int CurrentAnimalNumber => _animalIds.Count;
    public int Capacity { get; }
    
    public Enclosure(EnclosureType type, int size, int capacity)
    {
        Id = Guid.NewGuid();
        Type = type;
        Size = size;
        Capacity = capacity;
        _animalIds = new List<Guid>();
    }
    
    public bool AddAnimal(Guid animalId)
    {
        if (CurrentAnimalNumber >= Capacity)
        {
            return false;
        }
        
        _animalIds.Add(animalId);
        return true;
    }
    
    public bool DeleteAnimal(Guid animalId)
    {
        return _animalIds.Remove(animalId);
    }
}