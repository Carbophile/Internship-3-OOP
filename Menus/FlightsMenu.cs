using Internship_3_OOP.Entities;

namespace Internship_3_OOP.Menus;

public static class FlightsMenu
{
    public static void Menu()
    {
        while (true)
        {
            Console.Clear();

            foreach (var menuItem in Enum.GetValues<MenuItem>()) Console.WriteLine($"{(int)menuItem} - {menuItem}");

            if (Enum.TryParse<MenuItem>(Console.ReadLine(), true, out var command))
            {
                switch (command)
                {
                    case MenuItem.ViewAndSearch:
                        FlightFilteringMenu.Menu(Flight.All);
                        continue;
                    case MenuItem.Add:
                        AddFlight();
                        continue;
                    case MenuItem.EditById:
                        EditFlightById();
                        continue;
                    case MenuItem.DeleteById:
                        DeleteFlightById();
                        continue;
                    case MenuItem.Return:
                        return;
                }

                Helper.HandleInputError();
            }
        }
    }

    private static void AddFlight()
    {
        var departureAirport = GetAirport("Enter departure airport:");
        var arrivalAirport = GetAirport("Enter arrival airport:");
        var departureTime = GetDateTime("Enter departure time (YYYY-MM-DD HH:MM):");
        var arrivalTime = GetDateTime("Enter arrival time (YYYY-MM-DD HH:MM):");
        var crew = GetCrew();
        var plane = GetPlane();

        if (crew == null || plane == null)
        {
            Helper.HandleInputError("Invalid crew or plane selection.");
            return;
        }

        try
        {
            new Flight(departureAirport, arrivalAirport, departureTime, arrivalTime, crew, plane);
            Console.WriteLine("Flight added successfully!");
        }
        catch (ArgumentException e)
        {
            Helper.HandleInputError(e.Message);
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static string GetAirport(string message)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(message);
            var airport = Console.ReadLine();
            if (Flight.ValidateAirport(airport, true)) return airport!;
        }
    }

    private static DateTimeOffset GetDateTime(string message)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(message);
            if (DateTimeOffset.TryParse(Console.ReadLine(), out var dateTime)) return dateTime;
            Helper.HandleInputError("Invalid date and time format.");
        }
    }

    private static Crew? GetCrew()
    {
        var availableCrews = Crew.All.ToList();
        if (availableCrews.Count == 0)
        {
            Console.WriteLine("No crews available.");
            return null;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a crew:");
            for (var i = 0; i < availableCrews.Count; i++)
            {
                var crew = availableCrews[i];
                Console.WriteLine($"{i + 1}. Pilot: {crew.Pilot.FName}, Copilot: {crew.Copilot.FName}");
            }

            if (int.TryParse(Console.ReadLine(), out var choice) && choice > 0 && choice <= availableCrews.Count)
                return availableCrews[choice - 1];
            Helper.HandleInputError("Invalid choice.");
        }
    }

    private static Plane? GetPlane()
    {
        var availablePlanes = Plane.All.ToList();
        if (availablePlanes.Count == 0)
        {
            Console.WriteLine("No planes available.");
            return null;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a plane:");
            for (var i = 0; i < availablePlanes.Count; i++)
            {
                var plane = availablePlanes[i];
                Console.WriteLine($"{i + 1}. {plane.Name}");
            }

            if (int.TryParse(Console.ReadLine(), out var choice) && choice > 0 && choice <= availablePlanes.Count)
                return availablePlanes[choice - 1];
            Helper.HandleInputError("Invalid choice.");
        }
    }

    private static void EditFlightById()
    {
        Console.Clear();
        Console.WriteLine("Enter flight ID to edit:");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            var flight = Flight.All.FirstOrDefault(f => f.Id == id);
            if (flight != null)
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Editing flight:");
                    Console.WriteLine(
                        $"1. Departure Airport: {flight.DepartureAirport}");
                    Console.WriteLine(
                        $"2. Arrival Airport: {flight.ArrivalAirport}");
                    Console.WriteLine(
                        $"3. Departure Time: {flight.DepartureTime}");
                    Console.WriteLine(
                        $"4. Arrival Time: {flight.ArrivalTime}");
                    Console.WriteLine("5. Return");

                    if (Enum.TryParse<EditMenuItem>(Console.ReadLine(), true, out var choice))
                        switch (choice)
                        {
                            case EditMenuItem.DepartureAirport:
                                flight.DepartureAirport = GetAirport("Enter new departure airport:");
                                continue;
                            case EditMenuItem.ArrivalAirport:
                                flight.ArrivalAirport = GetAirport("Enter new arrival airport:");
                                continue;
                            case EditMenuItem.DepartureTime:
                                flight.UpdateFlightTime(GetDateTime("Enter new departure time:"), flight.ArrivalTime);
                                continue;
                            case EditMenuItem.ArrivalTime:
                                flight.UpdateFlightTime(flight.DepartureTime, GetDateTime("Enter new arrival time:"));
                                continue;
                            case EditMenuItem.Return:
                                return;
                        }

                    Helper.HandleInputError();
                }

            Helper.HandleInputError("Flight not found.");
        }
        else
        {
            Helper.HandleInputError("Invalid ID format.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void DeleteFlightById()
    {
        Console.Clear();
        Console.WriteLine("Enter flight ID to delete:");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            var flight = Flight.All.FirstOrDefault(f => f.Id == id);
            if (flight != null)
            {
                if (flight.Bookings.Count > flight.Plane.Capacity * 0.5)
                {
                    Helper.HandleInputError("Cannot delete a flight that is more than 50% full.");
                }
                else if (flight.DepartureTime > DateTimeOffset.Now.AddHours(24))
                {
                    flight.Delete();
                    Console.WriteLine("Flight deleted successfully.");
                }
                else
                {
                    Helper.HandleInputError(
                        "Cannot delete a flight that is in the past or departs in less than 24 hours.");
                }
            }
            else
            {
                Helper.HandleInputError("Flight not found.");
            }
        }
        else
        {
            Helper.HandleInputError("Invalid ID format.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private enum MenuItem
    {
        ViewAndSearch,
        Add,
        EditById,
        DeleteById,
        Return
    }

    private enum EditMenuItem
    {
        DepartureAirport = 1,
        ArrivalAirport = 2,
        DepartureTime = 3,
        ArrivalTime = 4,
        Return = 5
    }
}