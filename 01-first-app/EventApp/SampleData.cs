namespace EventApp;

public static class SampleData
{
    // Adds two sample events, but only when the table is empty,
    // so running the app twice does not create duplicates.
    public static int EnsureSeeded(EventsDbContext db)
    {
        if (db.Events.Any()) return 0;

        db.Events.Add(new Event { Title = "Intro to EF Core", StartsAt = new(2027, 1, 7, 18, 0, 0), Capacity = 50 });
        db.Events.Add(new Event { Title = "Civil Service Review Night", StartsAt = new(2027, 1, 14, 19, 0, 0), Capacity = 120 });

        return db.SaveChanges();   // nothing is stored until this line runs
    }
}
