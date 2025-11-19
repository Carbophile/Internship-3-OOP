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
                        var newPassenger = new Passenger(fname, lname, birthDate, gender, email, password);
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
                throw new NotImplementedException();
            }

            Helper.HandleInputError("Invalid email or password");
        }
    }
}