using EventApp;
using Microsoft.EntityFrameworkCore;

// Tell EF Core to use SQLite, and where the database file lives.
var options = new DbContextOptionsBuilder<EventsDbContext>()
    .UseSqlite("Data Source=events.db")
    .Options;

using var db = new EventsDbContext(options);

// Part 1 only: create the database if it does not exist.
// Part 2 replaces this with migrations.
db.Database.EnsureCreated();

var added = SampleData.EnsureSeeded(db);
Console.WriteLine(added == 0 ? "Sample events already exist." : $"Saved {added} events.");

Console.WriteLine();
Console.WriteLine("Events, soonest first:");
foreach (var e in db.Events.OrderBy(e => e.StartsAt))
{
    Console.WriteLine($"  {e.Id}. {e.Title}  ({e.StartsAt:yyyy-MM-dd HH:mm}, {e.Capacity} seats)");
}
