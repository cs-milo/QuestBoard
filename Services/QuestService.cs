using Microsoft.EntityFrameworkCore;
using QuestBoard.Models;

namespace QuestBoard.Services
{
    public class QuestService : IQuestService
    {
        private readonly QuestBoardContext _db;
        public QuestService(QuestBoardContext db) => _db = db;

        public async Task<List<Quest>> GetListAsync(string? search = null)
        {
            var q = _db.Quests
                       .Include(x => x.Game)
                       .Include(x => x.PlayerQuests)
                           .ThenInclude(pq => pq.Player)
                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(x =>
                    x.Title.Contains(search) ||
                    x.Name.Contains(search) ||
                    (x.Description ?? "").Contains(search) ||
                    (x.Game != null && x.Game.Name.Contains(search)) ||
                    x.PlayerQuests.Any(pq => pq.Player.Name.Contains(search)));
            }

            return await q.OrderBy(x => x.IsCompleted)
                          .ThenBy(x => x.DueDateUtc)
                          .ToListAsync();
        }

        public Task<Quest?> GetAsync(int id) =>
            _db.Quests
               .Include(x => x.Game)
               .Include(x => x.PlayerQuests)
                   .ThenInclude(pq => pq.Player)
               .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Quest> CreateAsync(Quest quest)
        {
            _db.Quests.Add(quest);
            await _db.SaveChangesAsync();
            return quest;
        }

        public async Task<bool> UpdateAsync(Quest quest)
        {
            if (!await _db.Quests.AnyAsync(x => x.Id == quest.Id)) return false;
            _db.Quests.Update(quest);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Quests.FindAsync(id);
            if (entity is null) return false;
            _db.Quests.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
