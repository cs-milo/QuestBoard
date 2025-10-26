using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;

namespace QuestBoard.Controllers
{
    public class GamesController : Controller
    {
        private readonly QuestBoardContext _ctx;

        public GamesController(QuestBoardContext ctx)
        {
            _ctx = ctx;
        }

        // GET: /Games
        public async Task<IActionResult> Index()
        {
            return View(await _ctx.Games.AsNoTracking().ToListAsync());
        }

        // GET: /Games/Create
        public IActionResult Create() => View();

        // POST: /Games/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (!ModelState.IsValid) return View(game);

            _ctx.Games.Add(game);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Games/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _ctx.Games.FindAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        // POST: /Games/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.Id) return NotFound();
            if (!ModelState.IsValid) return View(game);

            _ctx.Update(game);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _ctx.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return NotFound();
            return View(game);
        }

        // GET: /Games/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _ctx.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return NotFound();
            return View(game);
        }

        // POST: /Games/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _ctx.Games.FindAsync(id);
            if (game != null)
            {
                _ctx.Games.Remove(game);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
