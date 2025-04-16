namespace ZooManagerWeb.Application.Services;

using ZooManagerWeb.Domain.Events;
using ZooManagerWeb.Infrastructure.Interfaces;

public class AnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    // Делегат для публикации событий
    public delegate void EventHandler<T>(T eventArgs) where T : class;

    // Событие для подписки на перемещение животного
    public event EventHandler<AnimalMovedEvent> AnimalMoved;

    public AnimalTransferService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
    }

    // Перемещение животного между вольерами
    public async Task<bool> TransferAnimalAsync(Guid animalId, Guid targetEnclosureId)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId);
        var targetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId);

        if (animal == null || targetEnclosure == null)
        {
            return false;
        }

        if (targetEnclosure.CurrentAnimalNumber >= targetEnclosure.Capacity)
        {
            return false;
        }

        Guid sourceEnclosureId = animal.CurrentEnclosureId ?? Guid.Empty;

        if (animal.CurrentEnclosureId.HasValue)
        {
            var sourceEnclosure = await _enclosureRepository.GetByIdAsync(animal.CurrentEnclosureId.Value);
            if (sourceEnclosure != null)
            {
                sourceEnclosure.DeleteAnimal(animalId);
                await _enclosureRepository.UpdateAsync(sourceEnclosure);
            }
        }

        targetEnclosure.AddAnimal(animalId);
        await _enclosureRepository.UpdateAsync(targetEnclosure);

        animal.UpdateEnclosure(targetEnclosureId);
        await _animalRepository.UpdateAsync(animal);

        var movedEvent = new AnimalMovedEvent(animal.Id, sourceEnclosureId, targetEnclosure.Id);
        OnAnimalMoved(movedEvent);

        return true;
    }

    // Метод для вызова события
    protected virtual void OnAnimalMoved(AnimalMovedEvent e)
    {
        AnimalMoved?.Invoke(e);
    }
}