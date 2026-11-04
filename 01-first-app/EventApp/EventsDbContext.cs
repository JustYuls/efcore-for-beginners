using Microsoft.EntityFrameworkCore;

namespace EventApp;

// A DbContext is one session with the database.
public class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options)
{
    // Each DbSet is a table you can query and save to.
    public DbSet<Event> Events => Set<Event>();
}
