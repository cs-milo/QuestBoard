# Week 10 — Entity Framework Core Integration (QuestBoard)

This update introduces a durable data layer for QuestBoard using Entity Framework Core and SQL Server LocalDB. The goal for Week 10 is narrow by design: define the domain model, register it in the application, create a migration that matches the model, and prove that the database builds and stores rows. The UI and extra features stay out of scope so the foundation is stable for later weeks.

## What changed in Week 10

- **Entities**
  - **Game** — `Id, Name, Genre, CreatedAt`
  - **Player** — `Id, Name, Class, Level, GameId` (each player belongs to one game)
  - **Quest** — `Id, Title, Description, RewardGold, Difficulty, GameId` (each quest belongs to one game)
  - **PlayerQuest** — join table for the many-to-many between players and quests (`PlayerId, QuestId, AcceptedAt, IsCompleted`)

- **DbContext**
  - `QuestBoardContext` exposes `DbSet<Game>`, `DbSet<Player>`, `DbSet<Quest>`, and `DbSet<PlayerQuest>`.
  - Relationships configured in `OnModelCreating`:
    - Composite primary key on `PlayerQuest (PlayerId, QuestId)`.
    - `Game → Players` and `Game → Quests` use **Cascade**.
    - `PlayerQuest → Player` and `PlayerQuest → Quest` use **Restrict/NoAction** to avoid SQL Server’s “multiple cascade paths” error.

- **Migrations**
  - A migration was created and applied to generate the four tables above.
  - The database builds without errors; proof screenshots are included.

## How to run locally

1. Ensure the .NET SDK and Visual Studio (with ASP.NET workload) are installed.  
2. The default connection string in `appsettings.json` targets LocalDB:  
   `Server=(localdb)\\MSSQLLocalDB;Database=QuestBoardDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`
3. Build the schema:
   - **Visual Studio:** *Tools → NuGet Package Manager → Package Manager Console* → `Update-Database`
   - **or** terminal at the project folder: `dotnet ef database update`
4. Start the app:
   - **Visual Studio:** press **F5**
   - **or** terminal: `dotnet run`
5. Browse to `https://localhost:<port>/`. If you scaffolded CRUD, visit `/Games`, `/Players`, `/Quests` to create and view rows.

## Verifying the database

- **SQL Server Object Explorer** → `(localdb)\MSSQLLocalDB → Databases → QuestBoardDb → Tables`  
- Right-click **Games**, **Players**, **Quests**, **PlayerQuests** → **View Data**.  
- You should see actual values (not just null placeholders). Screenshots below capture the exact state used for submission.

## Evidence (screenshots)

- ![Migration Output](docs/week10/Wk10_MigrationOutput.png)
- ![Games Data](docs/week10/Wk10_Games.png)
- ![Players Data](docs/week10/Wk10_Players.png)
- ![Quests Data](docs/week10/Wk10_Quests.png)

## File pointers

These filenames are present in the project:
- **DbContext:** `QuestBoardContext.cs`
- **Entities:** `Game.cs`, `Player.cs`, `Quest.cs`, `PlayerQuest.cs`
- **Relationships & seeding:** inside `OnModelCreating` in `QuestBoardContext`
- **Migrations:** files under the `Migrations` folder (initial schema + fixes)

> Exact relative paths can vary by solution layout (for example, you might keep the app under `src/`); the filenames above are consistent.

## Notes for later weeks

The model is intentionally minimal. Future work can build on this layer by adding view models, services, and pages for assigning quests to players. Because the delete behavior was set deliberately (Cascade at Game, Restrict at the join), future changes won’t fight SQL Server’s multiple-cascade rule.

## Submission links

- **Repository (entities, DbContext, migration commits):** link to this branch/PR.  
- **Week 10 README:** this file, `README.week10.md`, in the repository root.
