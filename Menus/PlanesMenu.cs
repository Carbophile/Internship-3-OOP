using Internship_3_OOP.Entities;

namespace Internship_3_OOP.Menus;

public static class PlanesMenu
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
                        ListAllPlanes();
                        continue;
                    case MenuItem.Add:
                        AddNewPlane();
                        continue;
                    case MenuItem.Search:
                        SearchPlanes();
                        continue;
                    case MenuItem.Delete:
                        DeletePlane();
                        continue;
                    case MenuItem.Return:
                        return;
                }

            Helper.HandleInputError();
        }
    }

    private static void ListAllPlanes()
    {
        Console.Clear();
        var planes = Plane.All;
        if (planes.Count == 0)
        {
            Console.WriteLine("No planes available.");
        }
        else
        {
            Console.WriteLine("Available planes:");
            for (var i = 0; i < planes.Count; i++)
            {
                var plane = planes[i];
                Console.WriteLine(
                    $"{i + 1}. Name: {plane.Name}, Manufacture Date: {plane.ManufactureDate}, Classes: {string.Join(", ", plane.Classes)}");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void AddNewPlane()
    {
        var name = GetPlaneName();
        var manufactureDate = GetManufactureDate();
        var classes = GetClasses();
        var capacity = GetCapacity();

        while (true)
        {
            Console.Clear();

            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Manufacture Date: {manufactureDate}");
            Console.WriteLine($"Classes: {string.Join(", ", classes)}");
            Console.WriteLine($"Capacity: {capacity}");

            Console.WriteLine("\nConfirm adding this plane? (Y/N)");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Y:
                    var newPlane = new Plane(name, manufactureDate, classes, capacity);
                    Console.WriteLine("\nPlane added successfully! Press any key to continue...");
                    Console.ReadKey();
                    return;
                case ConsoleKey.N:
                    Console.WriteLine("\nPlane addition cancelled. Press any key to return to the previous menu...");
                    Console.ReadKey();
                    return;
                default:
                    Helper.HandleInputError("Invalid input. Please enter Y or N");
                    break;
            }
        }
    }

    private static void SearchPlanes()
    {
        Console.Clear();
        Console.WriteLine("Search by: 1. ID, 2. Name");
        if (int.TryParse(Console.ReadLine(), out var choice))
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter ID:");
                    if (Guid.TryParse(Console.ReadLine(), out var id))
                    {
                        var plane = Plane.All.FirstOrDefault(p => p.Id == id);
                        if (plane != null)
                            Console.WriteLine(
                                $"Plane found: Name: {plane.Name}, Manufacture Date: {plane.ManufactureDate}, Classes: {string.Join(", ", plane.Classes)}");
                        else
                            Console.WriteLine("Plane not found.");
                    }
                    else
                    {
                        Helper.HandleInputError("Invalid ID format.");
                    }

                    break;
                case 2:
                    Console.WriteLine("Enter Name:");
                    var name = Console.ReadLine();
                    if (Helper.ValidateString(name))
                    {
                        var planes = Plane.All.Where(p => p.Name.Contains(name!, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        if (planes.Count > 0)
                        {
                            Console.WriteLine("Found planes:");
                            foreach (var plane in planes)
                                Console.WriteLine(
                                    $"- Name: {plane.Name}, Manufacture Date: {plane.ManufactureDate}, Classes: {string.Join(", ", plane.Classes)}");
                        }
                        else
                        {
                            Console.WriteLine("No planes found with that name.");
                        }
                    }

                    break;
                default:
                    Helper.HandleInputError("Invalid choice.");
                    break;
            }
        else
            Helper.HandleInputError("Invalid input.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void DeletePlane()
    {
        Console.Clear();
        Console.WriteLine("Delete by: 1. ID, 2. Name");
        if (int.TryParse(Console.ReadLine(), out var choice))
        {
            Plane? planeToDelete = null;
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter ID:");
                    if (Guid.TryParse(Console.ReadLine(), out var id))
                        planeToDelete = Plane.All.FirstOrDefault(p => p.Id == id);
                    else
                        Helper.HandleInputError("Invalid ID format.");

                    break;
                case 2:
                    Console.WriteLine("Enter Name:");
                    var name = Console.ReadLine();
                    if (Helper.ValidateString(name))
                    {
                        var planes = Plane.All.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        if (planes.Count == 1)
                            planeToDelete = planes[0];
                        else if (planes.Count > 1)
                            Console.WriteLine("Multiple planes found with that name. Please delete by ID.");
                        else
                            Console.WriteLine("No plane found with that name.");
                    }

                    break;
                default:
                    Helper.HandleInputError("Invalid choice.");
                    break;
            }

            if (planeToDelete != null)
            {
                Console.WriteLine(
                    $"Are you sure you want to delete plane: {planeToDelete.Name}? (Y/N)");
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    planeToDelete.Delete();
                    Console.WriteLine("Plane deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
            else if (choice == 1 || choice == 2)
            {
                Console.WriteLine("Plane not found.");
            }
        }
        else
        {
            Helper.HandleInputError("Invalid input.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    private static string GetPlaneName()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter plane name:");
            var name = Console.ReadLine();
            if (Helper.ValidateString(name, true)) return name!;
        }
    }

    private static DateOnly GetManufactureDate()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter manufacture date (YYYY-MM-DD):");
            var date = Helper.ParseDate(Console.ReadLine(), true);
            if (date != null && Helper.ValidateDate(date.Value, true)) return date.Value;
        }
    }

    private static List<Plane.Class> GetClasses()
    {
        var classes = new List<Plane.Class>();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a class to add (or 0 to finish):");
            foreach (var classOption in Enum.GetValues<Plane.Class>())
                Console.WriteLine($"{(int)classOption + 1} - {classOption}");

            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                if (choice == 0)
                {
                    if (classes.Count > 0) return classes;
                    Helper.HandleInputError("At least one class must be selected.");
                }
                else if (choice > 0 && choice <= Enum.GetValues<Plane.Class>().Length)
                {
                    var selectedClass = (Plane.Class)(choice - 1);
                    if (!classes.Contains(selectedClass))
                    {
                        classes.Add(selectedClass);
                        Console.WriteLine($"{selectedClass} added. Press any key to add another class or finish.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Helper.HandleInputError("Class already selected.");
                    }
                }
                else
                {
                    Helper.HandleInputError();
                }
            }
            else
            {
                Helper.HandleInputError();
            }
        }
    }

    private static int GetCapacity()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter plane capacity:");
            if (int.TryParse(Console.ReadLine(), out var capacity) && capacity > 0) return capacity;
            Helper.HandleInputError("Invalid capacity. Must be a positive number.");
        }
    }


    private enum MenuItem
    {
        ListAll,
        Add,
        Search,
        Delete,
        Return
    }
}