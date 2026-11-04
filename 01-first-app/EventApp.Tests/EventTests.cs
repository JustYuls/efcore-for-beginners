using EventApp;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Tests;

// Each test gets its own in-memory SQLite database.
// The connection must stay open, or the database disappears.
public sealed class EventTests : IDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private readonly DbContextOptions<EventsDbContext> _options;

    public EventTests()
    {
        _connection.Open();
        _options = new DbContextOptionsBuilder<EventsDbContext>().UseSqlite(_connection).Options;
        using var db = new EventsDbContext(_options);
        db.Database.EnsureCreated();
    }

    public void Dispose() => _connection.Dispose();

    private EventsDbContext NewContext() => new(_options);

    [Fact]
    public void Saved_event_can_be_read_back_with_a_new_context()
    {
        using (var db = NewContext())
        {
            db.Events.Add(new Event { Title = "Workshop", StartsAt = new DateTime(2027, 2, 1, 9, 0, 0), Capacity = 30 });
            db.SaveChanges();
        }

        using var fresh = NewContext();
        var found = fresh.Events.Single();

        Assert.True(found.Id > 0);               // the database assigned the key
        Assert.Equal("Workshop", found.Title);
        Assert.Equal(30, found.Capacity);
    }

    [Fact]
    public void Forgetting_SaveChanges_stores_nothing()
    {
        using (var db = NewContext())
        {
            db.Events.Add(new Event { Title = "Ghost event", StartsAt = DateTime.Today, Capacity = 10 });
            // no SaveChanges here: this is the common mistake
        }

        using var fresh = NewContext();
        Assert.Empty(fresh.Events);
    }

    [Fact]
    public void Events_come_back_soonest_first()
    {
        using (var db = NewContext())
        {
            db.Events.Add(new Event { Title = "Later", StartsAt = new DateTime(2027, 3, 1), Capacity = 1 });
            db.Events.Add(new Event { Title = "Sooner", StartsAt = new DateTime(2027, 1, 1), Capacity = 1 });
            db.SaveChanges();
        }

        using var fresh = NewContext();
        var titles = fresh.Events.OrderBy(e => e.StartsAt).Select(e => e.Title).ToList();

        Assert.Equal(new[] { "Sooner", "Later" }, titles);
    }

    [Fact]
    public void Seeding_twice_does_not_create_duplicates()
    {
        using var db = NewContext();

        var first = SampleData.EnsureSeeded(db);
        var second = SampleData.EnsureSeeded(db);

        Assert.Equal(2, first);
        Assert.Equal(0, second);
        Assert.Equal(2, db.Events.Count());
    }
}
