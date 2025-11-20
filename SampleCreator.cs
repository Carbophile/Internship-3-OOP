using Internship_3_OOP.Entities;

namespace Internship_3_OOP;

public static class SampleCreator
{
    public static void CreateSamples()
    {
        var pilot1 = new CrewMember("John", "Smith", new DateOnly(1980, 5, 15), Person<CrewMember>.Sex.Male,
            CrewMember.Position.Pilot);
        var copilot1 = new CrewMember("Emily", "Jones", new DateOnly(1985, 8, 22), Person<CrewMember>.Sex.Female,
            CrewMember.Position.Copilot);
        var flightAttendant1 = new CrewMember("Michael", "Williams", new DateOnly(1990, 2, 10),
            Person<CrewMember>.Sex.Male, CrewMember.Position.FlightAttendant);
        var flightAttendant2 = new CrewMember("Jessica", "Brown", new DateOnly(1992, 11, 30),
            Person<CrewMember>.Sex.Female, CrewMember.Position.FlightAttendant);

        var crew1 = new Crew(pilot1, copilot1, flightAttendant1, flightAttendant2);

        var plane1 = new Plane("Boeing 737", new DateOnly(2010, 1, 1),
            new Dictionary<Plane.Class, int> { { Plane.Class.Economy, 150 }, { Plane.Class.Business, 30 } });
        var plane2 = new Plane("Airbus A320", new DateOnly(2015, 6, 1),
            new Dictionary<Plane.Class, int>
                { { Plane.Class.Economy, 120 }, { Plane.Class.Business, 20 }, { Plane.Class.First, 10 } });

        var flight1 = new Flight("LDZA", "LFPG", DateTimeOffset.Now.AddDays(2),
            DateTimeOffset.Now.AddDays(2).AddHours(2), crew1, plane1, 1080);
        var flight2 = new Flight("LDZA", "EGLL", DateTimeOffset.Now.AddDays(3),
            DateTimeOffset.Now.AddDays(3).AddHours(2), crew1, plane2, 1320);
        var flight3 = new Flight("LDZA", "EDDF", DateTimeOffset.Now.AddHours(12), DateTimeOffset.Now.AddHours(14),
            crew1, plane1, 770);
        var flight4 = new Flight("LDZA", "EHAM", DateTimeOffset.Now.AddDays(4),
            DateTimeOffset.Now.AddDays(4).AddHours(2), crew1, plane2, 1100);

        var passengers = new List<Passenger>();
        for (var i = 0; i < 100; i++)
            passengers.Add(new Passenger($"Passenger{i}", $"Test{i}", new DateOnly(1990, 1, 1),
                Person<Passenger>.Sex.Male, $"passenger{i}@test.com", "password"));

        for (var i = 0; i < 20; i++) new Booking(passengers[i], flight1, Plane.Class.Economy);

        for (var i = 0; i < 100; i++) new Booking(passengers[i], flight2, Plane.Class.Economy);

        for (var i = 0; i < 75; i++) new Booking(passengers[i], flight4, Plane.Class.Economy);

        for (var i = 0; i < 20; i++) new Booking(passengers[i], flight3, Plane.Class.Economy);
    }
}