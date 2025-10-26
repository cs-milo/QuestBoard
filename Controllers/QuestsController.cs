using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;

namespace QuestBoard.Controllers
{
    public class QuestsController : Controller
    {
        private readonly QuestBoardContext _context;

        public QuestsController(QuestBoardContext context)
        {
            _context = context;
        }

        // GET: Quests
        public async Task<IActionResult> Index()
        {
            var quests = _context.Quests.Include(q => q.Game);
            return View(await quests.ToListAsync());
        }

        // GET: Quests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var quest = await _context.Quests
                .Include(q => q.Game)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (quest == null) return NotFound();

            return View(quest);
        }

        // GET: Quests/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name");
            return View();
        }

        // POST: Quests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,RewardGold,Difficulty,GameId")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", quest.GameId);
            return View(quest);
        }

        // GET: Quests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null) return NotFound();

            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", quest.GameId);
            return View(quest);
        }

        // POST: Quests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,RewardGold,Difficulty,GameId")] Quest quest)
        {
            if (id != quest.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Quests.Any(e => e.Id == quest.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", quest.GameId);
            return View(quest);
        }

        // GET: Quests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var quest = await _context.Quests
                .Include(q => q.Game)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (quest == null) return NotFound();

            return View(quest);
        }

        // POST: Quests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quest = await _context.Quests
                .Include(q => q.PlayerQuests)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quest == null)
                return NotFound();

            // Delete any PlayerQuest relationships first
            _context.PlayerQuests.RemoveRange(quest.PlayerQuests);
            _context.Quests.Remove(quest);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
