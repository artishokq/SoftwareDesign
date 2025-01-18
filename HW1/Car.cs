namespace HW1;

public class Car
{
    private static readonly Random _random = new Random();
    public int Number { get; }
    public Engine Engine { get; }

    public Car(int number)
    {
        Number = number;
        Engine = new Engine(_random.Next(1, 10));
    }

    public override string ToString()
    {
        return $"Номер: {Number}, Размер педалей: {Engine.Size}";
    }
}