namespace QuestBoard.ViewModels
{
    public class QuestListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDateUtc { get; set; }
        public bool IsCompleted { get; set; }

        public string? GameName { get; set; }

        public string? GameTitle { get; set; }
        public string? PlayerName { get; set; }
    }
}
