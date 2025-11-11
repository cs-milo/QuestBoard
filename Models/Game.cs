using System.ComponentModel.DataAnnotations;

namespace QuestBoard.Models
{
    public class Game
    {
        public int Id { get; set; }

        // Needed by generic helpers and dropdowns
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Required by Games views
        [StringLength(50)]
        public string? Genre { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<Quest> Quests { get; set; } = new List<Quest>();
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
