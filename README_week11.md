## Week 11 – Separation of Concerns / Dependency Injection

This week I implemented a service layer and integrated it via ASP.NET Core’s built-in Dependency Injection (DI) to separate non-UI logic from MVC controllers. The goal was to keep controllers thin and move data access and business logic into a dedicated service that can be reused and tested independently.

I added an IPlayerService interface and a PlayerService implementation. The service encapsulates queries such as listing all players with their related Game, retrieving a single player with related Game and Quests, creating, updating, and deleting players, as well as a simple business method (GetTopByLevelAsync) that returns the top N players by level. The service uses the existing QuestBoardContext from Entity Framework Core and is registered with the DI container using AddScoped, which is the recommended lifetime for EF Core per-request usage.

In PlayersController, I replaced direct data access with calls to the service, leaving UI-focused responsibilities (like populating dropdowns for Game selection) inside the controller. As a result, the controller methods are shorter, easier to read, and focused on HTTP + view concerns. The PlayerService centralizes data logic and query shape, which is better for maintainability and reduces duplication across the codebase.

This pattern directly supports real-world needs: services can be unit tested with an in-memory database or mocked interfaces; swapping implementations (e.g., caching, auditing, or API-based data) becomes straightforward; and the controller remains stable as behavior evolves. The new /Players/Top endpoint demonstrates how adding features now primarily means extending the service rather than complicating the controller.

How to verify:
1) Run the app → Players.
2) Click “Show Top 3 Players” or use the Top form.
3) Create/Edit/Delete still work as before; data persists in SQL Server.

Evidence:
- Service: `Services/IPlayerService.cs`, `Services/PlayerService.cs`
- DI: `Program.cs` (AddScoped)
- Controller usage: `Controllers/PlayersController.cs`
- UI Entry: `Views/Players/Index.cshtml`
