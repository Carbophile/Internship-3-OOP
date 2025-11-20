namespace Internship_3_OOP.Entities;

public sealed class CrewMember(
    string fName,
    string lName,
    DateOnly birthDate,
    Person<CrewMember>.Sex gender,
    CrewMember.Position crewPosition)
    : Person<CrewMember>(fName, lName, birthDate, gender)
{
    public enum Position
    {
        Pilot,
        Copilot,
        FlightAttendant
    }

    public Position CrewPosition
    {
        get;
        set
        {
            field = value;
            UpdateLastChanged();
        }
    } = crewPosition;

    public Crew? Crew
    {
        get;
        set
        {
            field = value;
            UpdateLastChanged();
        }
    }
}