namespace Internship_3_OOP;

public static class Helper
{
    public static void HandleInputError()
    {
        Console.Clear();

        Console.WriteLine("Invalid input! Press any key to try again...");
        Console.ReadKey();
    }
}