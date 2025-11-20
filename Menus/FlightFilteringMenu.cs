using Internship_3_OOP.Entities;

namespace Internship_3_OOP.Menus;

public static class FlightFilteringMenu
{
    private static void ShowFlights(IEnumerable<Flight> flights)
    {
        var flightsList = flights.ToList();
        if (flightsList.Count == 0)
        {
            Console.WriteLine("No flights found.");
        }
        else
        {
            Console.WriteLine("Found flights:");
            for (var i = 0; i < flightsList.Count; i++)
            {
                var flight = flightsList[i];
                var availableSeats = string.Join(", ",
                    flight.Plane.ClassCapacities.Select(kv =>
                        $"{kv.Key}: {kv.Value - flight.Bookings.Count(b => b.FlightClass == kv.Key)}"));
                Console.WriteLine(
                    $"{i + 1}. From: {flight.DepartureAirport}, To: {flight.ArrivalAirport}, Distance: {flight.Distance}km, Departure: {flight.DepartureTime}, Duration: {flight.Duration:hh\\:mm}, Available Seats: [{availableSeats}]");
            }
        }
    }

    public static void Menu(IEnumerable<Flight> flights)
    {
        var filteredFlights = flights.ToList();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Search and Sort Flights ---");
            Console.WriteLine("1 - Filter by Departure Airport");
            Console.WriteLine("2 - Filter by Arrival Airport");
            Console.WriteLine("3 - Sort Options");
            Console.WriteLine("4 - Show Current Results");
            Console.WriteLine("5 - Reset Filters and Sorters");
            Console.WriteLine("0 - Return");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter departure airport:");
                    var departure = Console.ReadLine();
                    filteredFlights = filteredFlights.Where(f =>
                        string.IsNullOrEmpty(departure) ||
                        f.DepartureAirport.Equals(departure, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "2":
                    Console.WriteLine("Enter arrival airport:");
                    var arrival = Console.ReadLine();
                    filteredFlights = filteredFlights.Where(f =>
                        string.IsNullOrEmpty(arrival) ||
                        f.ArrivalAirport.Equals(arrival, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "3":
                    SortMenu(ref filteredFlights);
                    break;
                case "4":
                    Console.Clear();
                    ShowFlights(filteredFlights);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;
                case "5":
                    filteredFlights = flights.ToList();
                    Console.WriteLine("Filters and sorters have been reset. Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "0":
                    return;
                default:
                    Helper.HandleInputError();
                    break;
            }
        }
    }

    private static void SortMenu(ref List<Flight> flights)
    {
        Console.Clear();
        Console.WriteLine("--- Sort Options ---");
        Console.WriteLine("1 - By Departure Time (Asc)");
        Console.WriteLine("2 - By Departure Time (Desc)");
        Console.WriteLine("3 - By Flight Length (Asc)");
        Console.WriteLine("4 - By Flight Length (Desc)");
        Console.WriteLine("5 - By Distance (Asc)");
        Console.WriteLine("6 - By Distance (Desc)");
        Console.WriteLine("7 - By Departure Airport Name (Asc)");
        Console.WriteLine("8 - By Departure Airport Name (Desc)");
        Console.WriteLine("0 - Return");

        switch (Console.ReadLine())
        {
            case "1":
                flights = flights.OrderBy(f => f.DepartureTime).ToList();
                break;
            case "2":
                flights = flights.OrderByDescending(f => f.DepartureTime).ToList();
                break;
            case "3":
                flights = flights.OrderBy(f => f.Duration).ToList();
                break;
            case "4":
                flights = flights.OrderByDescending(f => f.Duration).ToList();
                break;
            case "5":
                flights = flights.OrderBy(f => f.Distance).ToList();
                break;
            case "6":
                flights = flights.OrderByDescending(f => f.Distance).ToList();
                break;
            case "7":
                flights = flights.OrderBy(f => f.DepartureAirport).ToList();
                break;
            case "8":
                flights = flights.OrderByDescending(f => f.DepartureAirport).ToList();
                break;
            case "0":
                return;
            default:
                Helper.HandleInputError();
                break;
        }

        Console.WriteLine("Sort applied. Press any key to continue...");
        Console.ReadKey();
    }
}