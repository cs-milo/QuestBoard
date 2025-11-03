using QuestBoard.Models;

namespace QuestBoard.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task CreateAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(int id);

        // Example query
        Task<List<Player>> GetTopByLevelAsync(int count);
    }
}
