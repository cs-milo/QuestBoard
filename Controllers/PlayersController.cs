using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;
using QuestBoard.Services;

namespace QuestBoard.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerService _players;
        private readonly QuestBoardContext _db; // only for dropdowns (Games)
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(IPlayerService players, QuestBoardContext db, ILogger<PlayersController> logger)
        {
            _players = players;
            _db = db;
            _logger = logger;
        }

        // GET: /Players
        public async Task<IActionResult> Index()
        {
            var list = await _players.GetAllAsync();
            return View(list);
        }

        // GET: /Players/Top?count=3  (example usage of service logic)
        public async Task<IActionResult> Top(int count = 3)
        {
            var list = await _players.GetTopByLevelAsync(count);
            return View("Index", list); // reuse Index view to display top players
        }

        // GET: /Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var player = await _players.GetByIdAsync(id.Value);
            if (player == null) return NotFound();
            return View(player);
        }

        // GET: /Players/Create
        public async Task<IActionResult> Create()
        {
            await PopulateGamesDropDown();
            return View();
        }

        // POST: /Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Class,Level,GameId")] Player player)
        {
            if (!ModelState.IsValid)
            {
                await PopulateGamesDropDown(player.GameId);
                return View(player);
            }

            try
            {
                await _players.CreateAsync(player);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Player");
                ModelState.AddModelError(string.Empty, "Save failed. Try again.");
                await PopulateGamesDropDown(player.GameId);
                return View(player);
            }
        }

        // GET: /Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _players.GetByIdAsync(id.Value);
            if (player == null) return NotFound();

            await PopulateGamesDropDown(player.GameId);
            return View(player);
        }

        // POST: /Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Class,Level,GameId")] Player player)
        {
            if (id != player.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await PopulateGamesDropDown(player.GameId);
                return View(player);
            }

            try
            {
                await _players.UpdateAsync(player);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Re-check existence
                var exists = await _players.GetByIdAsync(id);
                if (exists == null) return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing Player {PlayerId}", id);
                ModelState.AddModelError(string.Empty, "Save failed. Try again.");
                await PopulateGamesDropDown(player.GameId);
                return View(player);
            }
        }

        // GET: /Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var player = await _players.GetByIdAsync(id.Value);
            if (player == null) return NotFound();

            return View(player);
        }

        // POST: /Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _players.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Player {PlayerId}", id);
                ModelState.AddModelError(string.Empty, "Delete failed due to related data. Try again.");
                var reloaded = await _players.GetByIdAsync(id);
                return View("Delete", reloaded);
            }
        }

        // ===== Helpers (UI only) =====
        private async Task PopulateGamesDropDown(int? selectedGameId = null)
        {
            var games = await _db.Games.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
            ViewData["GameId"] = new SelectList(games, "Id", "Name", selectedGameId);
        }
    }
}
