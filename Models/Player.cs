using System.ComponentModel.DataAnnotations;

namespace QuestBoard.Models
{
    public class Player
    {
        public int Id { get; set; }

        // Needed by generic helpers and views
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Required by Players views
        [StringLength(50)]
        public string? Class { get; set; }

        public int? Level { get; set; }

        // Required by PlayersController & views
        public int? GameId { get; set; }
        public Game? Game { get; set; }

        [StringLength(100)]
        public string? Handle { get; set; }

        public ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();
    }
}
