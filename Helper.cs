namespace Internship_3_OOP;

public static class Helper
{
    public static void HandleInputError(string message = "Invalid input")
    {
        Console.Clear();

        Console.WriteLine($"{message}! Press any key to try again...");
        Console.ReadKey();
    }

    public static bool ValidateString(string? str, bool output = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            if (output) HandleInputError("Input cannot be empty");

            return false;
        }

        if (str.Length < 4)
        {
            if (output) HandleInputError("Input cannot be less than 4 characters");

            return false;
        }

        return true;
    }

    public static bool ValidateDate(DateOnly dateOnly, bool output = false)
    {
        var date = dateOnly.ToDateTime(TimeOnly.MinValue);

        if (date > DateTime.Now)
        {
            if (output) HandleInputError("Date cannot be in the future");

            return false;
        }

        if (date < DateTime.Now.AddYears(-65))
        {
            if (output) HandleInputError("Date cannot be more than 65 years in the past");

            return false;
        }

        return true;
    }

    public static bool ValidateEmail(string? email, bool output = false)
    {
        if (!ValidateString(email, output)) return false;

        if (!email!.Contains('@') || !email.Contains('.'))
        {
            if (output) HandleInputError("Invalid email format");

            return false;
        }

        return true;
    }

    public static DateOnly? ParseDate(string? date, bool output = false)
    {
        if (DateOnly.TryParse(date, out var dateOnly) && ValidateDate(dateOnly, output)) return dateOnly;
        if (output) HandleInputError("Invalid date format");
        return null;
    }
}