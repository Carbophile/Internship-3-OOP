using Internship_3_OOP.Entities;

namespace Internship_3_OOP.Menus;

public static class PassengersMenu
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
                    case MenuItem.Registration:
                        RegistrationMenu.Menu();
                        continue;
                    case MenuItem.Login:
                        LoginMenu.Menu();
                        continue;
                    case MenuItem.Return:
                        return;
                }

                Helper.HandleInputError();
            }
        }
    }

    private static string GetEmail()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("Enter your email:");
            var email = Console.ReadLine();

            if (Helper.ValidateEmail(email, true)) return email!;
        }
    }

    private static string GetPassword()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("Enter your password:");
            var password = Console.ReadLine();

            if (Helper.ValidateString(password, true)) return password!;
        }
    }

    private enum MenuItem
    {
        Registration,
        Login,
        Return
    }

    private static class RegistrationMenu
    {
        public static void Menu()
        {
            var fname = GetFirstName();
            var lname = GetLastName();
            var birthDate = GetBirthDate();
            var gender = GetGender();
            var email = GetEmail();
            var password = GetPassword();

            while (true)
            {
                Console.Clear();

                Console.WriteLine($"First name: {fname}");
                Console.WriteLine($"Last name: {lname}");
                Console.WriteLine($"Birth date: {birthDate}");
                Console.WriteLine($"Gender: {gender}");
                Console.WriteLine($"Email: {email}");
                Console.WriteLine($"Password: {password}"); // Such security

                Console.WriteLine("\nConfirm registration? (Y/N)");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        new Passenger(fname, lname, birthDate, gender, email, password);
                        Console.WriteLine("\nRegistration successful! Press any key to continue...");
                        Console.ReadKey();
                        return;
                    case ConsoleKey.N:
                        Console.WriteLine("\nRegistration cancelled. Press any key to return to the previous menu...");
                        Console.ReadKey();
                        return;
                    default:
                        Helper.HandleInputError("Invalid input. Please enter Y or N");
                        break;
                }
            }
        }

        private static string GetFirstName()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Enter your first name:");
                var fName = Console.ReadLine();

                if (Helper.ValidateString(fName, true)) return fName!;
            }
        }

        private static string GetLastName()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Enter your last name:");
                var lName = Console.ReadLine();

                if (Helper.ValidateString(lName, true)) return lName!;
            }
        }

        private static DateOnly GetBirthDate()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Enter your birth date (YYYY-MM-DD):");
                var birthDate = Helper.ParseDate(Console.ReadLine(), true);
                if (birthDate != null && Helper.ValidateDate(birthDate.Value, true))
                    return birthDate.Value;
            }
        }

        private static Person<Passenger>.Sex GetGender()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Select your gender:");
                foreach (var genderOption in Enum.GetValues<Person<Passenger>.Sex>())
                    Console.WriteLine($"{(int)genderOption} - {genderOption}");

                if (Enum.TryParse<Person<Passenger>.Sex>(Console.ReadLine(), true, out var gender)) return gender;
                Helper.HandleInputError();
            }
        }

        private static string GetEmail()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Enter your email:");
                var email = Console.ReadLine();

                if (Helper.ValidateEmail(email, true))
                {
                    if (Passenger.All.Any(p => p.Email == email))
                        Helper.HandleInputError("An account with this email already exists");
                    else
                        return email!;
                }
            }
        }

        private static string GetPassword()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Create a password:");
                var password = Console.ReadLine();

                if (Helper.ValidateString(password, true)) return password!;
            }
        }
    }

    private static class LoginMenu
    {
        public static void Menu()
        {
            var email = GetEmail();
            var password = GetPassword();

            Console.Clear();

            var passenger = Passenger.All.FirstOrDefault(p => p.Email == email && p.Password == password);

            if (passenger != null)
            {
                Console.WriteLine($"\nWelcome, {passenger.FName}! Press any key to continue...");
                Console.ReadKey();
                LoggedinMenu.Menu(passenger);
            }

            Helper.HandleInputError("Invalid email or password");
        }

        private static class LoggedinMenu
        {
            public static void Menu(Passenger passenger)
            {
                while (true)
                {
                    Console.Clear();
                    
                    foreach (var menuItem in Enum.GetValues<MenuItem>())
                        Console.WriteLine($"{(int)menuItem} - {menuItem}");

                    if (Enum.TryParse<MenuItem>(Console.ReadLine(), true, out var command))
                    {
                        switch (command)
                        {
                            case MenuItem.MyBookings:
                                MyBookings(passenger);
                                continue;
                            case MenuItem.Book:
                                Book(passenger);
                                continue;
                            case MenuItem.SearchFlights:
                                SearchFlights();
                                continue;
                            case MenuItem.CancelFlight:
                                CancelFlight(passenger);
                                continue;
                            case MenuItem.Logout:
                                return;
                        }
                    }

                    Helper.HandleInputError();
                }
            }

            private static void MyBookings(Passenger passenger)
            {
                Console.Clear();
                var bookings = passenger.Bookings;
                if (bookings.Count == 0)
                {
                    Console.WriteLine("You have no bookings.");
                }
                else
                {
                    Console.WriteLine("Your bookings:");
                    for (var i = 0; i < bookings.Count; i++)
                    {
                        var booking = bookings[i];
                        Console.WriteLine(
                            $"{i + 1}. Flight from {booking.Flight.DepartureAirport} to {booking.Flight.ArrivalAirport}, Class: {booking.FlightClass}, Distance: {booking.Flight.Distance}km, at {booking.Flight.DepartureTime}");
                    }
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            private static void Book(Passenger passenger)
            {
                Console.Clear();
                var availableFlights = Flight.All.Where(f => f.DepartureTime > DateTimeOffset.Now).ToList();
                if (availableFlights.Count == 0)
                {
                    Console.WriteLine("No available flights.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Available flights:");
                for (var i = 0; i < availableFlights.Count; i++)
                {
                    var flight = availableFlights[i];
                    Console.WriteLine(
                        $"{i + 1}. From: {flight.DepartureAirport}, To: {flight.ArrivalAirport}, Distance: {flight.Distance}km, Departure: {flight.DepartureTime}");
                }

                Console.WriteLine("Enter the number of the flight you want to book (or 0 to cancel):");
                if (int.TryParse(Console.ReadLine(), out var choice) && choice > 0 && choice <= availableFlights.Count)
                {
                    var flight = availableFlights[choice - 1];
                    
                    Console.WriteLine("Select a class:");
                    var k = 0;
                    foreach (var flightClass in flight.Plane.Classes)
                    {
                        Console.WriteLine($"{k++} - {flightClass}");
                    }

                    if (int.TryParse(Console.ReadLine(), out var classChoice) && classChoice >= 0 && classChoice < flight.Plane.Classes.Count)
                    {
                        new Booking(passenger, flight, flight.Plane.Classes[classChoice]);
                        Console.WriteLine("Booking successful! Press any key to continue...");
                    }
                    else
                    {
                        Helper.HandleInputError("Invalid class choice.");
                    }
                }
                else if (choice == 0)
                {
                    Console.WriteLine("Booking cancelled.");
                }
                else
                {
                    Helper.HandleInputError("Invalid flight choice.");
                }

                Console.ReadKey();
            }

            private static void SearchFlights()
            {
                Console.Clear();
                Console.WriteLine("Enter departure airport (optional):");
                var departure = Console.ReadLine();
                Console.WriteLine("Enter arrival airport (optional):");
                var arrival = Console.ReadLine();

                var filteredFlights = Flight.All.Where(f =>
                    (string.IsNullOrEmpty(departure) ||
                     f.DepartureAirport.Equals(departure, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(arrival) ||
                     f.ArrivalAirport.Equals(arrival, StringComparison.OrdinalIgnoreCase)) &&
                    f.DepartureTime > DateTimeOffset.Now).ToList();

                if (filteredFlights.Count == 0)
                {
                    Console.WriteLine("No flights found.");
                }
                else
                {
                    Console.WriteLine("Sort by: 1. Departure Airport Name, 2. List Order (default)");
                    var sortChoice = Console.ReadLine();
                    if (sortChoice == "1")
                    {
                        filteredFlights = filteredFlights.OrderBy(f => f.DepartureAirport).ToList();
                    }

                    Console.WriteLine("Found flights:");
                    for (var i = 0; i < filteredFlights.Count; i++)
                    {
                        var flight = filteredFlights[i];
                        Console.WriteLine(
                            $"{i + 1}. From: {flight.DepartureAirport}, To: {flight.ArrivalAirport}, Distance: {flight.Distance}km, Departure: {flight.DepartureTime}");
                    }
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            private static void CancelFlight(Passenger passenger)
            {
                Console.Clear();
                var bookings = passenger.Bookings.ToList();
                if (bookings.Count == 0)
                {
                    Console.WriteLine("You have no bookings to cancel.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Your bookings:");
                for (var i = 0; i < bookings.Count; i++)
                {
                    var booking = bookings[i];
                    Console.WriteLine(
                        $"{i + 1}. Flight from {booking.Flight.DepartureAirport} to {booking.Flight.ArrivalAirport}, Distance: {booking.Flight.Distance}km, at {booking.Flight.DepartureTime}");
                }

                Console.WriteLine("Enter the number of the booking you want to cancel (or 0 to return):");
                if (int.TryParse(Console.ReadLine(), out var choice) && choice > 0 && choice <= bookings.Count)
                {
                    var booking = bookings[choice - 1];
                    if ((booking.Flight.DepartureTime - DateTimeOffset.Now).TotalHours < 24)
                    {
                        Helper.HandleInputError("Cannot cancel a flight that departs in less than 24 hours.");
                    }
                    else
                    {
                        booking.Delete();
                        Console.WriteLine("Booking cancelled successfully.");
                    }
                }
                else if (choice == 0)
                {
                    Console.WriteLine("Cancellation aborted.");
                }
                else
                {
                    Helper.HandleInputError("Invalid choice.");
                }

                Console.ReadKey();
            }

            private enum MenuItem
            {
                MyBookings,
                Book,
                SearchFlights,
                CancelFlight,
                Logout
            }
        }
    }
}