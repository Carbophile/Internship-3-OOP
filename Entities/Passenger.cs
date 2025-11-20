namespace Internship_3_OOP.Entities;

public sealed class Passenger : Person<Passenger>
{
    private readonly List<Booking> _bookings = [];
    private readonly List<Flight> _favoriteFlights = [];

    public Passenger(string fName, string lName, DateOnly birthDate, Sex gender, string email, string password) : base(
        fName, lName, birthDate, gender)
    {
        Email = email;
        Password = password;
    }

    public IReadOnlyList<Booking> Bookings => _bookings;
    public IReadOnlyList<Flight> FavoriteFlights => _favoriteFlights;

    public string Email
    {
        get;
        set
        {
            if (Helper.ValidateEmail(value) && All.All(passenger => passenger.Email != value))
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

    public string Password
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
    } // Unhashed, plaintext, YOLO (just pretend it's Argon2id)

    public void AddBooking(Booking booking)
    {
        _bookings.Add(booking);
        UpdateLastChanged();
    }

    public void RemoveBooking(Booking booking)
    {
        _bookings.Remove(booking);
        UpdateLastChanged();
    }

    public void AddFavoriteFlight(Flight flight)
    {
        _favoriteFlights.Add(flight);
        UpdateLastChanged();
    }

    public void RemoveFavoriteFlight(Flight flight)
    {
        _favoriteFlights.Remove(flight);
        UpdateLastChanged();
    }

    public override void Delete()
    {
        foreach (var booking in _bookings) booking.Delete();
        base.Delete();
    }
}