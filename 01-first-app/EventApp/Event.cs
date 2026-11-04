namespace EventApp;

// An entity is a plain C# class. EF Core turns it into a table row.
public class Event
{
    public int Id { get; set; }                 // "Id" becomes the primary key by convention
    public string Title { get; set; } = "";
    public DateTime StartsAt { get; set; }
    public int Capacity { get; set; }
}
