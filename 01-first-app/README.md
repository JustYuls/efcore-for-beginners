# Part 1: Your First EF Core App

Part 1 of 10 in **EF Core for Beginners: From First Database to a Real API**.
Running project: an Event Management System. Database: SQLite only.

## What you learn

- What an ORM is, and what EF Core does for you
- An entity (`Event`), a `DbContext` and a `DbSet`
- Pointing EF Core at a SQLite file
- `Add`, `SaveChanges` and a simple query
- The most common beginner mistake: forgetting `SaveChanges`

## Run it

You need the .NET 10 SDK.

```powershell
cd EventApp
dotnet run
```

First run creates `events.db` and prints:

```
Saved 2 events.

Events, soonest first:
  1. Intro to EF Core  (2027-01-07 18:00, 50 seats)
  2. Civil Service Review Night  (2027-01-14 19:00, 120 seats)
```

Run it again and it prints `Sample events already exist.` Delete `events.db` to start over.

## Test it

```powershell
dotnet test EventApp.Tests
```

4 tests use an in-memory SQLite database, so they never touch `events.db`.

## Files

| File | What it is |
|---|---|
| `EventApp/Event.cs` | The entity: Id, Title, StartsAt, Capacity |
| `EventApp/EventsDbContext.cs` | The session with the database |
| `EventApp/SampleData.cs` | Adds two events once |
| `EventApp/Program.cs` | Builds options, creates the database, lists events |
| `EventApp.Tests/EventTests.cs` | 4 tests |

## Note

`EnsureCreated()` is only for this first part. Part 2 replaces it with migrations, which you need as soon as the model changes.

Next: Part 2, Migrations and Modeling.
