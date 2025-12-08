using QuestBoard.Models;

namespace QuestBoard.Services
{
    public interface IQuestService
    {
        Task<List<Quest>> GetListAsync(string? search = null);
        Task<Quest?> GetAsync(int id);
        Task<Quest> CreateAsync(Quest quest);
        Task<bool> UpdateAsync(Quest quest);
        Task<bool> DeleteAsync(int id);
        
        // Week 15 – stored procedure call
        Task<List<Quest>> GetOpenQuestsForGameAsync(int gameId);
    }
}
