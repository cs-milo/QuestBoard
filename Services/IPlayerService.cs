using QuestBoard.Models;

namespace QuestBoard.Services
{
    public interface IPlayerService
    {
        // Existing/basic ops
        Task<List<Player>> GetListAsync(string? search = null);
        Task<Player?> GetAsync(int id);
        Task<Player> CreateAsync(Player player);
        Task<bool> UpdateAsync(Player player);
        Task<bool> DeleteAsync(int id);
        Task<List<Game>> GetGamesAsync();

        // Methods required by controllers/views
        Task<Player?> GetByIdAsync(int id);
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetTopByLevelAsync(int count);
    }
}
