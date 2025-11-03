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

        public async Task<List<Player>> GetAllAsync()
        {
            return await _db.Players
                .Include(p => p.Game)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _db.Players
                .Include(p => p.Game)
                .Include(p => p.PlayerQuests).ThenInclude(pq => pq.Quest)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Player player)
        {
            _db.Players.Add(player);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            _db.Players.Update(player);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var player = await _db.Players
                .Include(p => p.PlayerQuests)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return;

            if (player.PlayerQuests?.Any() == true)
                _db.PlayerQuests.RemoveRange(player.PlayerQuests);

            _db.Players.Remove(player);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Player>> GetTopByLevelAsync(int count)
        {
            return await _db.Players
                .Include(p => p.Game)
                .OrderByDescending(p => p.Level)
                .ThenBy(p => p.Name)
                .Take(Math.Max(1, count))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
