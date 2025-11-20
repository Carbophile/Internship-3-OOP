namespace Internship_3_OOP.Entities;

public sealed class Plane : Entity<Plane>
{
    public enum Class
    {
        Economy,
        Business,
        First
    }

    private readonly List<Flight> _flights = [];

    public Plane(string name, DateOnly manufactureDate, Dictionary<Class, int> classCapacities)
    {
        Name = name;
        ManufactureDate = manufactureDate;
        ClassCapacities = classCapacities;
    }

    public string Name
    {
        get;
        set
        {
            if (Helper.ValidateString(value))
            {
                field = value;
                UpdateLastChanged();
            }
            else
            {
                throw new ArgumentException(value);
            }
        }
    }

    public DateOnly ManufactureDate
    {
        get;
        set
        {
            if (Helper.ValidateDate(value))
            {
                field = value;
                UpdateLastChanged();
            }
            else
            {
                throw new ArgumentException(value.ToString());
            }
        }
    }

    public Dictionary<Class, int> ClassCapacities { get; set; }

    public IReadOnlyList<Flight> Flights => _flights;

    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
        UpdateLastChanged();
    }

    public void RemoveFlight(Flight flight)
    {
        _flights.Remove(flight);
        UpdateLastChanged();
    }

    public override void Delete()
    {
        foreach (var flight in _flights.ToList()) flight.Delete();
        base.Delete();
    }
}