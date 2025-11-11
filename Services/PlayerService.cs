using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;

namespace QuestBoard.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly QuestBoardContext _db;

        public PlayerService(QuestBoardContext db)
        {
            _db = db;
        }

        // ---------- Basic list with optional search ----------
        public async Task<List<Player>> GetListAsync(string? search = null)
        {
            var q = _db.Players.Include(p => p.Game).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Class ?? "").Contains(search) ||
                    (p.Handle ?? "").Contains(search) ||
                    (p.Game != null && p.Game.Name.Contains(search)));
            }

            return await q.OrderBy(p => p.Name).ToListAsync();
        }

        // ---------- Single fetch used by older code ----------
        public Task<Player?> GetAsync(int id) =>
            _db.Players.Include(p => p.Game).FirstOrDefaultAsync(p => p.Id == id);

        // ---------- Methods required by your controllers/views ----------
        public Task<Player?> GetByIdAsync(int id) => GetAsync(id);

        public Task<List<Player>> GetAllAsync() =>
            _db.Players.Include(p => p.Game).OrderBy(p => p.Name).ToListAsync();

        public Task<List<Player>> GetTopByLevelAsync(int count) =>
            _db.Players
               .Include(p => p.Game)
               .OrderByDescending(p => (int?)p.Level ?? 0)
               .ThenBy(p => p.Name)
               .Take(count)
               .ToListAsync();

        // ---------- CUD ----------
        public async Task<Player> CreateAsync(Player player)
        {
            _db.Players.Add(player);
            await _db.SaveChangesAsync();
            return player;
        }

        public async Task<bool> UpdateAsync(Player player)
        {
            if (!await _db.Players.AnyAsync(p => p.Id == player.Id)) return false;
            _db.Players.Update(player);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Players.FindAsync(id);
            if (entity == null) return false;
            _db.Players.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<List<Game>> GetGamesAsync() =>
            _db.Games.OrderBy(g => g.Name).ToListAsync();
    }
}
