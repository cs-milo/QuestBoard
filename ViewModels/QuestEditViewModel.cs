using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuestBoard.ViewModels
{
    public class QuestEditViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime? DueDateUtc { get; set; }
        public bool IsCompleted { get; set; }

        public int? GameId { get; set; }
        public string? GameTitle { get; set; }

        // ✅ what the view is asking for
        public SelectList? GameOptions { get; set; }

        // Some templates bind a single player to a quest
        public int? PlayerId { get; set; }
        public SelectList? PlayerOptions { get; set; }
    }
}
