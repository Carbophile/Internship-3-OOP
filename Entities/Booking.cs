namespace Internship_3_OOP.Entities;

public sealed class Booking: Entity<Booking>
{
    public Passenger Passenger { get; }
    public Flight Flight { get; }

    public override void Delete()
    {
        Passenger.RemoveBooking(this);
        Flight.RemoveBooking(this);
        base.Delete();
    }
    
    public Booking(Passenger passenger, Flight flight)
    {
        Passenger = passenger;
        Flight = flight;
        passenger.AddBooking(this);
        Flight.AddBooking(this);
    }
}