namespace QuestBoard.ViewModels
{
    public class QuestDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime? DueDateUtc { get; set; }
        public bool IsCompleted { get; set; }

        public int? GameId { get; set; }
        public string? GameName { get; set; }

        // ✅ what the view is asking for
        public string? GameTitle { get; set; }
        public string? PlayerName { get; set; }

        // optional full list for display
        public List<string> PlayerNames { get; set; } = new();
    }
}
