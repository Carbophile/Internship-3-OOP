namespace Internship_3_OOP.Entities;

public sealed class Booking : Entity<Booking>
{
    public Booking(Passenger passenger, Flight flight, Plane.Class flightClass)
    {
        Passenger = passenger;
        Flight = flight;
        FlightClass = flightClass;
        passenger.AddBooking(this);
        Flight.AddBooking(this);
    }

    public Passenger Passenger { get; }
    public Flight Flight { get; }
    public Plane.Class FlightClass { get; }

    public override void Delete()
    {
        Passenger.RemoveBooking(this);
        Flight.RemoveBooking(this);
        base.Delete();
    }
}