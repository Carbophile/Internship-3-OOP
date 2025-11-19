using Internship_3_OOP.Menus;

namespace Internship_3_OOP;

public static class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Clear();

            foreach (var menuItem in Enum.GetValues<MenuItem>()) Console.WriteLine($"{(int)menuItem} - {menuItem}");

            if (Enum.TryParse<MenuItem>(Console.ReadLine(), true, out var command))
                switch (command)
                {
                    case MenuItem.Passengers:
                        PassengersMenu.Menu();
                        continue;
                    case MenuItem.Flights:
                        FlightsMenu.Menu();
                        continue;
                    case MenuItem.Planes:
                        PlanesMenu.Menu();
                        continue;
                    case MenuItem.Crew:
                        CrewMenu.Menu();
                        continue;
                    case MenuItem.Quit:
                        return;
                }

            Helper.HandleInputError();
        }
    }

    private enum MenuItem
    {
        Quit,
        Passengers,
        Flights,
        Planes,
        Crew
    }
}