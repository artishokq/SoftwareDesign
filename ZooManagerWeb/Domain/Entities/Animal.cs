namespace ZooManagerWeb.Domain.Entities;

using ZooManagerWeb.Domain.Value_Object.Animal;

public class Animal
{
    public Guid Id { get; }
    public string Name { get; }
    public string Species { get; }
    public Gender Gender { get; }
    public DateTime Birthday { get; }
    public string FavFood { get; }
    public HealthStatus HealthStatus { get; private set; }
    public Guid? CurrentEnclosureId { get; private set; }

    public Animal(string species, string name, DateTime birthdate, Gender gender,
        string favFood, HealthStatus healthStatus)
    {
        Id = Guid.NewGuid();
        Species = species;
        Name = name;
        Birthday = birthdate;
        Gender = gender;
        FavFood = favFood;
        HealthStatus = healthStatus;
    }

    public void Heal()
    {
        if (HealthStatus == HealthStatus.Sick)
        {
            HealthStatus = HealthStatus.Healthy;
        }
    }

    public void UpdateEnclosure(Guid? newEnclosureId)
    {
        CurrentEnclosureId = newEnclosureId;
    }

    public void Feed()
    {
    }
}