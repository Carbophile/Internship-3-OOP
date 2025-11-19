namespace Internship_3_OOP.Entities;

public sealed class Passenger : Person<Passenger>
{
    private readonly List<Booking> _bookings = [];

    public Passenger(string fName, string lName, DateOnly birthDate, Sex gender, string email, string password) : base(
        fName, lName, birthDate, gender)
    {
        if (!Helper.ValidateEmail(email)) throw new ArgumentException(email);
        if (!Helper.ValidateString(password)) throw new ArgumentException(password);

        if (All.Any(passenger => passenger.Email == email)) throw new ArgumentException(email);

        Email = email;
        Password = password;
    }

    public IReadOnlyList<Booking> Bookings => _bookings;

    public string Email
    {
        get;
        set
        {
            if (Helper.ValidateEmail(value))
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
    }
}