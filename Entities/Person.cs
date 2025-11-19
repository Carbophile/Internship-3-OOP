namespace Internship_3_OOP.Entities;

public abstract class Person<TSelf> : Entity<TSelf> where TSelf : Person<TSelf>
{
    public enum Sex
    {
        Male,
        Female,
        Other
    }

    protected Person(string fName, string lName, DateOnly birthDate, Sex gender)
    {
        if (!Helper.ValidateString(fName))
            throw new ArgumentException(fName);
        if (!Helper.ValidateString(lName))
            throw new ArgumentException(lName);
        if (!Helper.ValidateDate(birthDate))
            throw new ArgumentException(birthDate.ToString());

        FName = fName;
        LName = lName;
        BirthDate = birthDate;
        Gender = gender;
    }

    public string FName
    {
        get;
        set
        {
            if (Helper.ValidateString(value))
            {
                field = value;
                UpdateLastChanged();
            }
            else
            {
                throw new ArgumentException(value);
            }
        }
    }

    public string LName
    {
        get;
        set
        {
            if (Helper.ValidateString(value))
            {
                field = value;
                UpdateLastChanged();
            }
            else
            {
                throw new ArgumentException(value);
            }
        }
    }

    public DateOnly BirthDate
    {
        get;
        set
        {
            if (Helper.ValidateDate(value))
            {
                field = value;
                UpdateLastChanged();
            }
            else
            {
                throw new ArgumentException(value.ToString());
            }
        }
    }

    public Sex Gender
    {
        get;
        set
        {
            field = value;
            UpdateLastChanged();
        }
    }
}