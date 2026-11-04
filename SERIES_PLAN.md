# EF Core for Beginners: Build an Event Management App with C# and .NET 10

Plan v2 (decisions confirmed). Nothing is recorded or uploaded yet.

## Confirmed decisions

| Item | Decision |
|---|---|
| Length | 10 parts |
| Database | SQLite only |
| Running project | **Event Management System** |
| Viewer level | Intermediate C# (knows classes, interfaces, generics, LINQ basics, async/await). New to EF Core and databases-from-code |
| Series and repo name | EF Core for Beginners: Build an Event Management App with C# and .NET 10. Repo: `efcore-for-beginners` (public, MIT) |
| Stack | .NET 10 (SDK 10.0.401 is installed on this PC), EF Core 10, xUnit, `dotnet ef` tool |
| Format | Same as the MCP series: about 5 min per part, male Piper voice, burned-in captions, numbered files, PART N OF 10 thumbnails, YouTube kit, Shorts |

"Intermediate" means no Part 0 and a faster pace: we do not explain what a class or a lambda is. We still explain every database and EF idea from zero.

## The running project: Event Management System

An app for organizers who run events (seminars, workshops, review sessions) and track who signs up.

| Entity | Fields (core) | Relationships |
|---|---|---|
| `Organizer` | Id, Name, Email | one organizer has many events |
| `Venue` | Id, Name, City, Capacity | one venue has many events |
| `Event` | Id, Title, StartsAt, EndsAt, Price, Status | belongs to Organizer and Venue; has many Tags and many Registrations |
| `Tag` | Id, Name | many-to-many with Event |
| `Attendee` | Id, FullName, Email | has many Registrations |
| `Registration` | Id, EventId, AttendeeId, RegisteredAt, Status | join entity between Event and Attendee, with its own data |
| `EventDetails` | EventId, Description, Agenda | one-to-one with Event |

Questions the finished app answers:
- Which events are coming up this month, and how many seats are left?
- Who registered for an event, and who is on the waiting list?
- Which organizer has the most attendees?
- Revenue per event.

## Part-by-part outline

Every part: goal, what the viewer builds, key concepts, one common mistake, and what Part N+1 adds.

### Part 1: Your First EF Core App
- **Goal:** Save and read an `Event` from SQLite in a console app.
- **Concepts:** what an ORM is, `DbContext`, `DbSet`, entity classes, SQLite provider, connection string, `Add`, `SaveChanges`, simple query, where the `.db` file lives.
- **Build:** `Event` entity, `EventsDbContext`, create the database, insert two events, list them.
- **Common mistake:** forgetting `SaveChanges`, so nothing is stored.
- **Next:** change the model without losing data (migrations).

### Part 2: Migrations and Modeling
- **Goal:** Describe the database in C# and evolve it safely.
- **Concepts:** `dotnet ef migrations add` and `database update`, what a migration file is, primary keys, required fields, max length, Data Annotations vs Fluent API, `IEntityTypeConfiguration`, seed data (`HasData`), why not `EnsureCreated` for real apps.
- **Build:** add `Venue` and `Organizer`, configure `Event`, seed sample data, add a column in a second migration.
- **Common mistake:** editing an applied migration instead of adding a new one.
- **Next:** connect the entities.

### Part 3: Relationships
- **Goal:** Model how events, venues, attendees and tags relate.
- **Concepts:** one-to-many, many-to-many (skip navigations and an explicit join entity), one-to-one, navigation properties, foreign keys, required vs optional, delete behavior (cascade, restrict).
- **Build:** Organizer/Venue to Event, Event to Tag, `Registration` as a join entity with extra data, `EventDetails` one-to-one.
- **Common mistake:** assuming cascade delete is always on or always safe.
- **Next:** asking questions with LINQ.

### Part 4: Querying with LINQ
- **Goal:** Get exactly the data you need.
- **Concepts:** `Where`, `OrderBy`, `Select` projections to DTOs, `Include` and `ThenInclude`, `FirstOrDefault` vs `SingleOrDefault`, `Any`, `Count`, grouping, paging with `Skip` and `Take`, `AsNoTracking`, deferred execution, client vs server evaluation.
- **Build:** "upcoming events", "seats left", "revenue per event", paged event list.
- **Common mistake:** calling `ToList()` too early and filtering in memory.
- **Next:** changing data correctly.

### Part 5: Saving Data Safely
- **Goal:** Add, update and delete without surprises.
- **Concepts:** the change tracker and entity states, tracked vs untracked updates, `Remove`, transactions (`SaveChanges` is one; explicit transactions for several), concurrency tokens (`RowVersion` pattern for SQLite using a version field), handling `DbUpdateConcurrencyException`, unique indexes and handling constraint errors.
- **Build:** register an attendee with a seat check, no double registration, two organizers editing the same event.
- **Common mistake:** checking capacity and then inserting without protection against two users at once.
- **Next:** making it fast.

### Part 6: Fast Queries
- **Goal:** Find and fix slow EF code.
- **Concepts:** logging the generated SQL (`LogTo`, `EnableSensitiveDataLogging` in development only), the N+1 problem, `Include` vs projection, split queries, indexes (`HasIndex`), `ExecuteUpdate` and `ExecuteDelete` for bulk changes, compiled queries mentioned briefly.
- **Build:** a deliberately slow attendee list, measure, fix three ways, compare.
- **Common mistake:** adding `Include` everywhere "just in case".
- **Next:** put it behind an API.

### Part 7: EF Core in a Web API
- **Goal:** Expose events over HTTP.
- **Concepts:** ASP.NET Core Minimal API, `AddDbContext` and the scoped lifetime, dependency injection, async methods (`ToListAsync`, `SaveChangesAsync`), DTOs vs entities, validation, returning proper status codes, `ProblemDetails`.
- **Build:** endpoints to list, get, create and cancel events, and register an attendee.
- **Common mistake:** returning entities directly, causing cycles and over-exposure.
- **Next:** prove it works with tests.

### Part 8: Testing EF Core Code
- **Goal:** Tests you can trust.
- **Concepts:** xUnit, SQLite in-memory with an open connection, why the "InMemory" provider can hide bugs (no real constraints), a fresh database per test, test data builders, `WebApplicationFactory` for API tests.
- **Build:** tests for capacity rules, unique registration, and an endpoint.
- **Common mistake:** using the InMemory provider and thinking foreign keys are tested.
- **Next:** real-world features.

### Part 9: Real-World Features
- **Goal:** The features real apps need.
- **Concepts:** global query filters for soft delete (cancelled events), value converters (an enum stored as text, money in cents), JSON columns for flexible data, auditing with `SaveChanges` override or interceptors (`CreatedAt`, `UpdatedAt`), configuration and secrets, what changes when you move to another database (conceptual, no second DB used).
- **Build:** soft delete, audit timestamps, `EventStatus` stored as text.
- **Common mistake:** forgetting `IgnoreQueryFilters` when an admin needs deleted data.
- **Next:** put it all together.

### Part 10: Capstone: Event Management API
- **Goal:** One finished, tested app.
- **Concepts:** review of parts 1 to 9, project structure, migrations in a deployment script, a final checklist, what to learn next (PostgreSQL or SQL Server providers, Dapper vs EF, Blazor front end).
- **Build:** full feature set, README, all tests green.
- **Common mistake:** shipping with development logging on.

## Scope boundaries

**In scope:** code-first EF Core, migrations, relationships, LINQ, change tracking, transactions, concurrency, performance basics, Minimal API, testing, soft delete, converters, interceptors, JSON columns.

**Out of scope (said on screen):** database-first scaffolding, stored procedures and raw SQL beyond one safe mention, other database providers in practice, Blazor or MVC front ends, cloud deployment, EF6 and .NET Framework.

## Deliverables per part

- MP4 (about 5 min) with burned-in captions and an SRT
- Thumbnail with a PART N OF 10 badge
- YouTube kit: title, description, tags, chapters, pinned comment, setup sheet
- Code folder: `01-first-app` through `10-capstone`, each with tests, README, LICENSE
- 1 to 2 Shorts per part

Plus series playlist and description, GitHub README, upload checklist, and schedule.

## Schedule (Thursdays, after the December campaign)

| Part | Date | Part | Date |
|---|---|---|---|
| 1 | 7 Jan 2027 | 6 | 11 Feb 2027 |
| 2 | 14 Jan | 7 | 18 Feb |
| 3 | 21 Jan | 8 | 25 Feb |
| 4 | 28 Jan | 9 | 4 Mar |
| 5 | 4 Feb | 10 | 11 Mar |

## Build order and quality checks

1. For each part: write the code, run `dotnet build` and `dotnet test`, and only then write the script from the working code.
2. Slides and real terminal output, with a synthetic narration and an animated presenter (Sir Yuls).
3. Pin versions in `global.json` and the project files so the code still runs when .NET updates.
