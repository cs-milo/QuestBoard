using System.ComponentModel.DataAnnotations;

namespace QuestBoard.Models
{
    public class Quest
    {
        public int Id { get; set; }

        // Your UI uses Title, but some generic code expects Name as well
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Due (UTC)")]
        public DateTime? DueDateUtc { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }

        // Required by errors that reference Quest.Game
        public int? GameId { get; set; }
        public Game? Game { get; set; }

        public ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();
    }
}
