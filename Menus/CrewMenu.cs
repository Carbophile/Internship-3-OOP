using Internship_3_OOP.Entities;

namespace Internship_3_OOP.Menus;

public static class CrewMenu
{
    public static void Menu()
    {
        while (true)
        {
            Console.Clear();

            foreach (var menuItem in Enum.GetValues<MenuItem>()) Console.WriteLine($"{(int)menuItem} - {menuItem}");

            if (Enum.TryParse<MenuItem>(Console.ReadLine(), true, out var command))
                switch (command)
                {
                    case MenuItem.ListAll:
                        ListAllCrews();
                        continue;
                    case MenuItem.Create:
                        CreateNewCrew();
                        continue;
                    case MenuItem.AddMember:
                        AddNewCrewMember();
                        continue;
                    case MenuItem.Return:
                        return;
                }

            Helper.HandleInputError();
        }
    }

    private static void ListAllCrews()
    {
        Console.Clear();
        var crews = Crew.All;
        if (crews.Count == 0)
        {
            Console.WriteLine("No crews available.");
        }
        else
        {
            Console.WriteLine("Available crews:");
            for (var i = 0; i < crews.Count; i++)
            {
                var crew = crews[i];
                Console.WriteLine(
                    $"{i + 1}. Pilot: {crew.Pilot.FName} {crew.Pilot.LName}, Copilot: {crew.Copilot.FName} {crew.Copilot.LName}, Attendants: {crew.FlightAttendants.Count}");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void CreateNewCrew()
    {
        Console.Clear();
        var pilot = SelectCrewMember(CrewMember.Position.Pilot, "Select a Pilot");
        if (pilot == null) return;

        var copilot = SelectCrewMember(CrewMember.Position.Copilot, "Select a Copilot");
        if (copilot == null) return;

        var attendant1 = SelectCrewMember(CrewMember.Position.FlightAttendant, "Select the first Flight Attendant");
        if (attendant1 == null) return;

        var attendant2 = SelectCrewMember(CrewMember.Position.FlightAttendant,
            "Select the second Flight Attendant", [attendant1]);
        if (attendant2 == null) return;

        Console.Clear();
        Console.WriteLine("You are about to create a new crew with the following members:");
        Console.WriteLine($"Pilot: {pilot.FName} {pilot.LName}");
        Console.WriteLine($"Copilot: {copilot.FName} {copilot.LName}");
        Console.WriteLine($"Flight Attendant: {attendant1.FName} {attendant1.LName}");
        Console.WriteLine($"Flight Attendant: {attendant2.FName} {attendant2.LName}");
        Console.WriteLine("Do you want to proceed? (Y/N)");

        if (Console.ReadKey(true).Key == ConsoleKey.Y)
            try
            {
                new Crew(pilot, copilot, attendant1, attendant2);
                Console.WriteLine("Crew created successfully! Press any key to continue...");
            }
            catch (ArgumentException e)
            {
                Helper.HandleInputError(e.Message);
            }
        else
            Console.WriteLine("Crew creation cancelled. Press any key to continue...");

        Console.ReadKey();
    }

    private static CrewMember? SelectCrewMember(CrewMember.Position position, string prompt,
        List<CrewMember>? excluded = null)
    {
        var availableMembers = CrewMember.All.Where(m =>
            m.CrewPosition == position && m.Crew == null && (excluded == null || !excluded.Contains(m))).ToList();

        if (availableMembers.Count == 0)
        {
            Console.WriteLine($"No available members for position: {position}");
            Console.ReadKey();
            return null;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            for (var i = 0; i < availableMembers.Count; i++)
            {
                var member = availableMembers[i];
                Console.WriteLine($"{i + 1}. {member.FName} {member.LName}");
            }

            Console.WriteLine("Enter your choice (or 0 to cancel):");
            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                if (choice == 0) return null;
                if (choice > 0 && choice <= availableMembers.Count) return availableMembers[choice - 1];
            }

            Helper.HandleInputError();
        }
    }

    private static void AddNewCrewMember()
    {
        var fName = GetFirstName();
        var lName = GetLastName();
        var birthDate = GetBirthDate();
        var gender = GetGender();
        var position = GetPosition();

        Console.Clear();
        Console.WriteLine("You are about to add a new crew member with the following details:");
        Console.WriteLine($"Name: {fName} {lName}");
        Console.WriteLine($"Birth Date: {birthDate}");
        Console.WriteLine($"Gender: {gender}");
        Console.WriteLine($"Position: {position}");
        Console.WriteLine("Do you want to proceed? (Y/N)");

        if (Console.ReadKey(true).Key == ConsoleKey.Y)
        {
            new CrewMember(fName, lName, birthDate, gender, position);
            Console.WriteLine("Crew member added successfully! Press any key to continue...");
        }
        else
        {
            Console.WriteLine("Action cancelled. Press any key to continue...");
        }

        Console.ReadKey();
    }

    private static string GetFirstName()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter first name:");
            var fName = Console.ReadLine();
            if (Helper.ValidateString(fName, true)) return fName!;
        }
    }

    private static string GetLastName()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter last name:");
            var lName = Console.ReadLine();
            if (Helper.ValidateString(lName, true)) return lName!;
        }
    }

    private static DateOnly GetBirthDate()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter birth date (YYYY-MM-DD):");
            var birthDate = Helper.ParseDate(Console.ReadLine(), true);
            if (birthDate != null && Helper.ValidateDate(birthDate.Value, true))
                return birthDate.Value;
        }
    }

    private static Person<CrewMember>.Sex GetGender()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select gender:");
            foreach (var genderOption in Enum.GetValues<Person<CrewMember>.Sex>())
                Console.WriteLine($"{(int)genderOption} - {genderOption}");

            if (Enum.TryParse<Person<CrewMember>.Sex>(Console.ReadLine(), true, out var gender)) return gender;
            Helper.HandleInputError();
        }
    }

    private static CrewMember.Position GetPosition()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select position:");
            foreach (var positionOption in Enum.GetValues<CrewMember.Position>())
                Console.WriteLine($"{(int)positionOption} - {positionOption}");

            if (Enum.TryParse<CrewMember.Position>(Console.ReadLine(), true, out var position)) return position;
            Helper.HandleInputError();
        }
    }

    private enum MenuItem
    {
        ListAll,
        Create,
        AddMember,
        Return
    }
}