using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;
using QuestBoard.Services;
using QuestBoard.ViewModels;

namespace QuestBoard.Controllers
{
    public class QuestsController : Controller
    {
        private readonly IQuestService _service;
        private readonly ILogger<QuestsController> _logger;
        private readonly QuestBoardContext _db;

        public QuestsController(IQuestService service, QuestBoardContext db, ILogger<QuestsController> logger)
        {
            _service = service;
            _db = db;
            _logger = logger;
        }

        // LIST
        public async Task<IActionResult> Index(string? search)
        {
            var correlationId = HttpContext.TraceIdentifier;

            _logger.LogInformation(
                "Quest Index requested. Search={Search}, CorrelationId={CorrelationId}",
                search,
                correlationId);

            var list = await _service.GetListAsync(search);

            var vms = list.Select(x => new QuestListItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Name = x.Name,
                Description = x.Description,
                DueDateUtc = x.DueDateUtc,
                IsCompleted = x.IsCompleted,
                GameName = x.Game?.Name,
                GameTitle = x.Game?.Name,
                PlayerName = x.PlayerQuests.Select(pq => pq.Player.Name).FirstOrDefault()
            }).ToList();

            ViewBag.Search = search;
            return View(vms);
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;

            var quest = await _db.Quests
                .Include(q => q.Game)
                .Include(q => q.PlayerQuests).ThenInclude(pq => pq.Player)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quest is null)
            {
                _logger.LogWarning(
                    "Quest Details not found. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    id,
                    correlationId);

                return NotFound();
            }

            var vm = new QuestDetailsViewModel
            {
                Id = quest.Id,
                Title = quest.Title,
                Name = quest.Name,
                Description = quest.Description,
                DueDateUtc = quest.DueDateUtc,
                IsCompleted = quest.IsCompleted,
                GameId = quest.GameId,
                GameName = quest.Game?.Name,
                GameTitle = quest.Game?.Name,
                PlayerName = quest.PlayerQuests.Select(pq => pq.Player.Name).FirstOrDefault(),
                PlayerNames = quest.PlayerQuests.Select(pq => pq.Player.Name).Distinct().ToList()
            };

            _logger.LogInformation(
                "Quest Details viewed. QuestId={QuestId}, CorrelationId={CorrelationId}",
                id,
                correlationId);

            return View(vm);
        }

        // CREATE (GET) – now uses QuestEditViewModel
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new QuestEditViewModel
            {
                DueDateUtc = DateTime.UtcNow.AddDays(7),
                IsCompleted = false
            };

            await PopulateGamesSelectList();
            await PopulatePlayersSelectList();

            vm.GameOptions = (SelectList)ViewBag.Games;
            vm.PlayerOptions = (SelectList)ViewBag.Players;

            return View(vm);
        }

        // CREATE (POST) – accepts QuestEditViewModel to match the view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestEditViewModel vm)
        {
            var correlationId = HttpContext.TraceIdentifier;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Quest Create validation failed. CorrelationId={CorrelationId}",
                    correlationId);

                await PopulateGamesSelectList(vm.GameId);
                await PopulatePlayersSelectList(vm.PlayerId);
                vm.GameOptions = (SelectList)ViewBag.Games;
                vm.PlayerOptions = (SelectList)ViewBag.Players;
                return View(vm);
            }

            // Map VM -> entity
            var quest = new Quest
            {
                Title = vm.Title,
                Name = vm.Name,
                Description = vm.Description,
                DueDateUtc = vm.DueDateUtc,
                IsCompleted = vm.IsCompleted,
                GameId = vm.GameId
            };

            try
            {
                await _service.CreateAsync(quest);

                // Attach a player if one was selected
                if (vm.PlayerId.HasValue)
                {
                    _db.PlayerQuests.Add(new PlayerQuest
                    {
                        PlayerId = vm.PlayerId.Value,
                        QuestId = quest.Id,
                        IsCompleted = quest.IsCompleted
                    });
                    await _db.SaveChangesAsync();
                }

                _logger.LogInformation(
                    "Quest Create succeeded. QuestId={QuestId}, Title={Title}, CorrelationId={CorrelationId}",
                    quest.Id,
                    quest.Title,
                    correlationId);

                TempData["Status"] = "Quest created.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Quest Create failed with exception. CorrelationId={CorrelationId}",
                    correlationId);

                ModelState.AddModelError(string.Empty, "Unable to create quest right now.");
                await PopulateGamesSelectList(vm.GameId);
                await PopulatePlayersSelectList(vm.PlayerId);
                vm.GameOptions = (SelectList)ViewBag.Games;
                vm.PlayerOptions = (SelectList)ViewBag.Players;
                return View(vm);
            }
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;

            var quest = await _db.Quests
                .Include(q => q.Game)
                .Include(q => q.PlayerQuests).ThenInclude(pq => pq.Player)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quest is null)
            {
                _logger.LogWarning(
                    "Quest Edit GET not found. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    id,
                    correlationId);

                return NotFound();
            }

            var vm = new QuestEditViewModel
            {
                Id = quest.Id,
                Title = quest.Title,
                Name = quest.Name,
                Description = quest.Description,
                DueDateUtc = quest.DueDateUtc,
                IsCompleted = quest.IsCompleted,
                GameId = quest.GameId,
                GameTitle = quest.Game?.Name,
                PlayerId = quest.PlayerQuests.Select(pq => (int?)pq.PlayerId).FirstOrDefault()
            };

            await PopulateGamesSelectList(vm.GameId);
            await PopulatePlayersSelectList(vm.PlayerId);

            vm.GameOptions = (SelectList)ViewBag.Games;
            vm.PlayerOptions = (SelectList)ViewBag.Players;

            return View(vm);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuestEditViewModel vm)
        {
            var correlationId = HttpContext.TraceIdentifier;

            if (id != vm.Id)
            {
                _logger.LogWarning(
                    "Quest Edit id mismatch. RouteId={RouteId}, ModelId={ModelId}, CorrelationId={CorrelationId}",
                    id,
                    vm.Id,
                    correlationId);

                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Quest Edit validation failed. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    vm.Id,
                    correlationId);

                await PopulateGamesSelectList(vm.GameId);
                await PopulatePlayersSelectList(vm.PlayerId);
                vm.GameOptions = (SelectList)ViewBag.Games;
                vm.PlayerOptions = (SelectList)ViewBag.Players;
                return View(vm);
            }

            var quest = await _db.Quests
                .Include(q => q.PlayerQuests)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quest is null)
            {
                _logger.LogWarning(
                    "Quest Edit entity not found. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    id,
                    correlationId);

                return NotFound();
            }

            quest.Title = vm.Title;
            quest.Name = vm.Name;
            quest.Description = vm.Description;
            quest.DueDateUtc = vm.DueDateUtc;
            quest.IsCompleted = vm.IsCompleted;
            quest.GameId = vm.GameId;

            if (vm.PlayerId.HasValue)
            {
                var existing = quest.PlayerQuests.FirstOrDefault(pq => pq.PlayerId == vm.PlayerId.Value);
                if (existing == null)
                {
                    quest.PlayerQuests.Clear();
                    quest.PlayerQuests.Add(new PlayerQuest
                    {
                        PlayerId = vm.PlayerId.Value,
                        QuestId = quest.Id,
                        IsCompleted = quest.IsCompleted
                    });
                }
            }
            else
            {
                quest.PlayerQuests.Clear();
            }

            try
            {
                await _service.UpdateAsync(quest);

                _logger.LogInformation(
                    "Quest Edit succeeded. QuestId={QuestId}, Title={Title}, CorrelationId={CorrelationId}",
                    quest.Id,
                    quest.Title,
                    correlationId);

                TempData["Status"] = "Quest updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Quest Edit failed with exception. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    quest.Id,
                    correlationId);

                ModelState.AddModelError(string.Empty, "Unable to save quest changes right now.");
                await PopulateGamesSelectList(vm.GameId);
                await PopulatePlayersSelectList(vm.PlayerId);
                vm.GameOptions = (SelectList)ViewBag.Games;
                vm.PlayerOptions = (SelectList)ViewBag.Players;
                return View(vm);
            }
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;

            try
            {
                await _service.DeleteAsync(id);

                _logger.LogInformation(
                    "Quest Delete succeeded. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    id,
                    correlationId);

                TempData["Status"] = "Quest deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Quest Delete failed with exception. QuestId={QuestId}, CorrelationId={CorrelationId}",
                    id,
                    correlationId);

                TempData["Status"] = "Unable to delete quest right now.";
            }

            return RedirectToAction(nameof(Index));
        }
        // WEEK 15: call stored procedure to list open quests for a given game
        public async Task<IActionResult> OpenByGame(int gameId)
        {
            var correlationId = HttpContext.TraceIdentifier;

            try
            {
                // Detect provider at runtime (SQLite vs SQL Server, etc.)
                var provider = _db.Database.ProviderName ?? string.Empty;

                List<Quest> quests;

                if (provider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    quests = await _db.Quests
                        .Where(q => q.GameId == gameId)
                        .AsNoTracking()
                        .ToListAsync();
                }
                else
                {
                    quests = await _db.Quests
                        .FromSqlInterpolated($"EXEC dbo.GetOpenQuestsForGame @GameId={gameId}")
                        .AsNoTracking()
                        .ToListAsync();
                }

                // Load game names
                var gameIds = quests.Select(q => q.GameId).Distinct().ToList();

                var gamesLookup = await _db.Games
                    .Where(g => gameIds.Contains(g.Id))
                    .ToDictionaryAsync(g => g.Id, g => g.Name);

                var vms = quests.Select(q => new QuestListItemViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Name = q.Name,
                    Description = q.Description,
                    DueDateUtc = q.DueDateUtc,
                    IsCompleted = q.IsCompleted,
                    GameName = q.GameId.HasValue && gamesLookup.TryGetValue(q.GameId.Value, out var name)
                        ? name
                        : null,
                    GameTitle = q.GameId.HasValue && gamesLookup.TryGetValue(q.GameId.Value, out var title)
                        ? title
                        : null,
                    PlayerName = null 
                }).ToList();

                _logger.LogInformation(
                    "OpenByGame succeeded. GameId={GameId}, Count={Count}, Provider={Provider}, CorrelationId={CorrelationId}",
                    gameId,
                    vms.Count,
                    provider,
                    correlationId);

                ViewBag.GameId = gameId;
                return View(vms);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "OpenByGame failed. GameId={GameId}, CorrelationId={CorrelationId}",
                    gameId,
                    correlationId);

                // No secrets to the user, details go to logs instead
                return StatusCode(500);
            }
        }


        // Helpers for dropdowns
        private async Task PopulateGamesSelectList(int? selectedGameId = null)
        {
            var games = await _db.Games.OrderBy(g => g.Name).ToListAsync();
            ViewBag.Games = new SelectList(games, nameof(Game.Id), nameof(Game.Name), selectedGameId);
        }

        private async Task PopulatePlayersSelectList(int? selectedPlayerId = null)
        {
            var players = await _db.Players.OrderBy(p => p.Name).ToListAsync();
            ViewBag.Players = new SelectList(players, nameof(Player.Id), nameof(Player.Name), selectedPlayerId);
        }
    }
}
