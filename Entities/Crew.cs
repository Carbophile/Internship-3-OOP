namespace Internship_3_OOP.Entities;

public sealed class Crew : Entity<Crew>
{
    private readonly List<Flight> _flights = new();

    public Crew(CrewMember pilot, CrewMember copilot, CrewMember flightAttendant1, CrewMember flightAttendant2)
    {
        if (pilot.CrewPosition != CrewMember.Position.Pilot)
            throw new ArgumentException("The first crew member must be a pilot.", nameof(pilot));
        if (copilot.CrewPosition != CrewMember.Position.Copilot)
            throw new ArgumentException("The second crew member must be a copilot.", nameof(copilot));
        if (flightAttendant1.CrewPosition != CrewMember.Position.FlightAttendant)
            throw new ArgumentException("The third crew member must be a flight attendant.", nameof(flightAttendant1));
        if (flightAttendant2.CrewPosition != CrewMember.Position.FlightAttendant)
            throw new ArgumentException("The fourth crew member must be a flight attendant.", nameof(flightAttendant2));

        var crewMembers = new List<CrewMember> { pilot, copilot, flightAttendant1, flightAttendant2 };
        if (crewMembers.Any(cm => cm.Crew != null))
            throw new ArgumentException("One or more crew members are already assigned to a crew.");

        Pilot = pilot;
        Copilot = copilot;
        FlightAttendants = [flightAttendant1, flightAttendant2];

        foreach (var member in crewMembers) member.Crew = this;
    }

    public CrewMember Pilot { get; }
    public CrewMember Copilot { get; }
    public IReadOnlyList<CrewMember> FlightAttendants { get; }
    public IReadOnlyList<Flight> Flights => _flights.AsReadOnly();

    public void AddFlight(Flight flight)
    {
        if (HasOverlappingFlights(flight))
            throw new InvalidOperationException(
                "This crew is already assigned to another flight in the same timeframe.");
        _flights.Add(flight);
    }

    public void RemoveFlight(Flight flight)
    {
        _flights.Remove(flight);
    }

    private bool HasOverlappingFlights(Flight newFlight)
    {
        return _flights.Any(existingFlight =>
            newFlight.DepartureTime < existingFlight.ArrivalTime &&
            newFlight.ArrivalTime > existingFlight.DepartureTime);
    }

    public override void Delete()
    {
        foreach (var flight in _flights.ToList()) flight.Delete();

        var crewMembers = new List<CrewMember> { Pilot, Copilot };
        crewMembers.AddRange(FlightAttendants);

        foreach (var member in crewMembers) member.Crew = null;

        base.Delete();
    }
}