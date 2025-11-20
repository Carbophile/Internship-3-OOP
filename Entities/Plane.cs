namespace Internship_3_OOP.Entities;

public sealed class Plane: Entity<Plane>
{
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
            if (Helper.ValidateDate(field))
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

    public IReadOnlyList<Class> Classes
    {
        get;
        set
        {
            if (Helper.ValidateList(value))
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

    private readonly List<Flight> _flights = [];

    public Plane(string name, DateOnly manufactureDate, IReadOnlyList<Class> classes)
    {
        this.Name = name;
        ManufactureDate = manufactureDate;
        Classes = classes;
    }
    
    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
    }

    public void RemoveFlight(Flight flight)
    {
        _flights.Remove(flight);
    }

    public override void Delete()
    {
        foreach (var flight in _flights) flight.Delete();
        base.Delete();
    }


    public enum Class
    {
        Economy,
        Business,
        First
    }
}