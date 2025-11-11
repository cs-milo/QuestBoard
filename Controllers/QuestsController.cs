using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;
using QuestBoard.Services;
using QuestBoard.ViewModels;
using System.Linq;

namespace QuestBoard.Controllers
{
    public class QuestsController : Controller
    {
        private readonly IQuestService _service;
        private readonly QuestBoardContext _db;

        public QuestsController(IQuestService service, QuestBoardContext db)
        {
            _service = service;
            _db = db;
        }

        // LIST
        public async Task<IActionResult> Index(string? search)
        {
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
            var quest = await _db.Quests
                .Include(q => q.Game)
                .Include(q => q.PlayerQuests).ThenInclude(pq => pq.Player)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quest is null) return NotFound();

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
                GameTitle = quest.Game?.Name, // ✅ expected by view
                PlayerName = quest.PlayerQuests.Select(pq => pq.Player.Name).FirstOrDefault(),
                PlayerNames = quest.PlayerQuests.Select(pq => pq.Player.Name).Distinct().ToList()
            };

            return View(vm);
        }

        // CREATE
        public async Task<IActionResult> Create()
        {
            await PopulateGamesSelectList();
            await PopulatePlayersSelectList();
            return View(new Quest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quest quest, int? playerId)
        {
            if (!ModelState.IsValid)
            {
                await PopulateGamesSelectList(quest.GameId);
                await PopulatePlayersSelectList(playerId);
                return View(quest);
            }

            await _service.CreateAsync(quest);

            // If a player was chosen in the Create view, attach via join table
            if (playerId.HasValue)
            {
                _db.PlayerQuests.Add(new PlayerQuest { PlayerId = playerId.Value, QuestId = quest.Id, IsCompleted = false });
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET) -> View expects QuestEditViewModel with dropdowns
        public async Task<IActionResult> Edit(int id)
        {
            var quest = await _db.Quests
                .Include(q => q.Game)
                .Include(q => q.PlayerQuests).ThenInclude(pq => pq.Player)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quest is null) return NotFound();

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
                // pick a representative PlayerId if any
                PlayerId = quest.PlayerQuests.Select(pq => (int?)pq.PlayerId).FirstOrDefault()
            };

            await PopulateGamesSelectList(vm.GameId);
            await PopulatePlayersSelectList(vm.PlayerId);

            // attach SelectLists to VM (what your view is binding to)
            vm.GameOptions = (SelectList)ViewBag.Games;
            vm.PlayerOptions = (SelectList)ViewBag.Players;

            return View(vm);
        }

        // EDIT (POST) -> accept VM, map back to entity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuestEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateGamesSelectList(vm.GameId);
                await PopulatePlayersSelectList(vm.PlayerId);
                vm.GameOptions = (SelectList)ViewBag.Games;
                vm.PlayerOptions = (SelectList)ViewBag.Players;
                return View(vm);
            }

            var quest = await _db.Quests
                .Include(q => q.PlayerQuests)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (quest is null) return NotFound();

            quest.Title = vm.Title;
            quest.Name = vm.Name;
            quest.Description = vm.Description;
            quest.DueDateUtc = vm.DueDateUtc;
            quest.IsCompleted = vm.IsCompleted;
            quest.GameId = vm.GameId;

            // Maintain a single Player link if the view is using a single selection
            if (vm.PlayerId.HasValue)
            {
                var existing = quest.PlayerQuests.FirstOrDefault(pq => pq.PlayerId == vm.PlayerId.Value);
                if (existing == null)
                {
                    // remove other links if you want strictly single selection
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
                // if none selected, clear associations (optional; matches single-select UX)
                quest.PlayerQuests.Clear();
            }

            await _service.UpdateAsync(quest);
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
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
