using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;

namespace QuestBoard.Controllers
{
    public class PlayersController : Controller
    {
        private readonly QuestBoardContext _context;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(QuestBoardContext context, ILogger<PlayersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Players
        public async Task<IActionResult> Index()
        {
            var players = await _context.Players
                .Include(p => p.Game)
                .AsNoTracking()
                .ToListAsync();

            return View(players);
        }

        // GET: /Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                .Include(p => p.Game)
                .Include(p => p.PlayerQuests)
                    .ThenInclude(pq => pq.Quest)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        // GET: /Players/Create
        public IActionResult Create()
        {
            PopulateGamesDropDown();
            return View();
        }

        // POST: /Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Class,Level,GameId")] Player player)
        {
            if (!ModelState.IsValid)
            {
                LogModelErrors();
                PopulateGamesDropDown(player.GameId);
                return View(player);
            }

            try
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Player");
                ModelState.AddModelError(string.Empty, "An error occurred while saving. Please try again.");
                PopulateGamesDropDown(player.GameId);
                return View(player);
            }
        }

        // GET: /Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            PopulateGamesDropDown(player.GameId);
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
                LogModelErrors();
                PopulateGamesDropDown(player.GameId);
                return View(player);
            }

            try
            {
                _context.Update(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PlayerExists(player.Id))
                    return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing Player {PlayerId}", player.Id);
                ModelState.AddModelError(string.Empty, "An error occurred while saving. Please try again.");
                PopulateGamesDropDown(player.GameId);
                return View(player);
            }
        }

        // GET: /Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                .Include(p => p.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        // POST: /Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Load player with related PlayerQuests so we can remove them first (avoids FK constraint errors)
            var player = await _context.Players
                .Include(p => p.PlayerQuests)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return NotFound();

            try
            {
                if (player.PlayerQuests?.Any() == true)
                    _context.PlayerQuests.RemoveRange(player.PlayerQuests);

                _context.Players.Remove(player);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Player {PlayerId}", id);
                ModelState.AddModelError(string.Empty, "Delete failed due to related data. Try again.");
                // Re-display the delete page with the player data
                var reloaded = await _context.Players
                    .Include(p => p.Game)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
                return View("Delete", reloaded);
            }
        }

        // ===== Helpers =====

        private void PopulateGamesDropDown(int? selectedGameId = null)
        {
            ViewData["GameId"] = new SelectList(
                _context.Games.AsNoTracking().OrderBy(g => g.Name),
                "Id",
                "Name",
                selectedGameId
            );
        }

        private void LogModelErrors()
        {
            foreach (var kv in ModelState)
            {
                foreach (var err in kv.Value.Errors)
                {
                    _logger.LogWarning("Model validation error on {Field}: {Error}", kv.Key, err.ErrorMessage);
                }
            }
        }

        private async Task<bool> PlayerExists(int id)
            => await _context.Players.AnyAsync(e => e.Id == id);
    }
}
