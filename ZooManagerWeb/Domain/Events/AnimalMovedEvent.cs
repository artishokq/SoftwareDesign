namespace ZooManagerWeb.Domain.Events;

public class AnimalMovedEvent
{
    public Guid AnimalId { get; }
    public DateTime MovedAtTime { get; }
    public Guid SourceEnclosureId { get; }
    public Guid TargetEnclosureId { get; }

    public AnimalMovedEvent(Guid animalId, Guid sourceEnclosureId, Guid targetEnclosureId)
    {
        AnimalId = animalId;
        SourceEnclosureId = sourceEnclosureId;
        TargetEnclosureId = targetEnclosureId;
        MovedAtTime = DateTime.UtcNow;
    }
}