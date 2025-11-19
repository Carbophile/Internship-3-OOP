namespace Internship_3_OOP.Entities;

public abstract class Entity<TSelf> where TSelf : Entity<TSelf>
{
    private static readonly List<TSelf> AllPrivate = [];

    protected Entity()
    {
        Id = Guid.NewGuid();
        Created = DateTimeOffset.Now;
        LastChanged = Created;

        AllPrivate.Add((TSelf)this);
    }

    public static IReadOnlyList<TSelf> All => AllPrivate;

    public Guid Id { get; }
    public DateTimeOffset LastChanged { get; private set; }
    public DateTimeOffset Created { get; }

    protected void UpdateLastChanged()
    {
        LastChanged = DateTimeOffset.Now;
    }

    public virtual void Delete()
    {
        AllPrivate.Remove((TSelf)this);
    }
}