namespace QuestBoard.Models
{
    public class PlayerQuest
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;

        public int QuestId { get; set; }
        public Quest Quest { get; set; } = default!;

        public bool IsCompleted { get; set; }
        public DateTime? CompletedUtc { get; set; }
    }
}
