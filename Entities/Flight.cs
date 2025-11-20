namespace Internship_3_OOP.Entities;

public sealed class Flight : Entity<Flight>
{
    private readonly List<Booking> _bookings = new();


    public Flight(string departureAirport, string arrivalAirport,
        DateTimeOffset departureTime, DateTimeOffset arrivalTime, Crew crew, Plane plane)
    {
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        UpdateFlightTime(departureTime, arrivalTime);
        Crew = crew;
        crew.AddFlight(this);
        Plane = plane;
        plane.AddFlight(this);
    }

    public Plane Plane
    {
        get;
        set
        {
            field = value;
            UpdateLastChanged();
        }
    }

    public long Distance
    {
        get;
        set
        {
            if (value > 0)
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

    public TimeSpan Duration => ArrivalTime - DepartureTime;

    public IReadOnlyList<Booking> Bookings => _bookings;

    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public DateTimeOffset DepartureTime { get; private set; }
    public DateTimeOffset ArrivalTime { get; private set; }
    public Crew Crew { get; }

    public void UpdateFlightTime(DateTimeOffset departure, DateTimeOffset arrival)
    {
        if (ValidateFlightTime(departure, arrival))
        {
            DepartureTime = departure;
            ArrivalTime = arrival;
        }
        else
        {
            throw new ArgumentException(departure + " " + arrival);
        }
    }

    public static bool ValidateAirport(string? airport, bool output = false)
    {
        if (!Helper.ValidateString(airport, output)) return false;

        if (airport!.Length != 4)
        {
            if (output) Helper.HandleInputError("Airport code must be 4 characters long");
            return false;
        }

        if (!airport.All(char.IsUpper))
        {
            if (output) Helper.HandleInputError("Airport code must contain only uppercase letters");
            return false;
        }

        if (!airport.All(char.IsLetter))
        {
            if (output) Helper.HandleInputError("Airport code must contain only letters");
            return false;
        }

        return true;
    }

    public static bool ValidateFlightTime(DateTimeOffset departure, DateTimeOffset arrival, bool output = false)
    {
        if (departure >= arrival)
        {
            if (output) Helper.HandleInputError("Departure time cannot be after or equal to arrival time");
            return false;
        }

        if (departure < DateTimeOffset.Now)
        {
            if (output) Helper.HandleInputError("Departure time cannot be in the past");
            return false;
        }

        return true;
    }

    public void AddBooking(Booking booking)
    {
        if (_bookings.Count >= Plane.Capacity) throw new InvalidOperationException("Flight is full.");
        _bookings.Add(booking);
    }

    public void RemoveBooking(Booking booking)
    {
        _bookings.Remove(booking);
    }

    public override void Delete()
    {
        foreach (var booking in _bookings.ToList()) booking.Delete();
        Plane.RemoveFlight(this);
        Crew.RemoveFlight(this);
        base.Delete();
    }
}