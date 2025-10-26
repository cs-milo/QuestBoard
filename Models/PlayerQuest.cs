namespace QuestBoard.Models
{
    public class PlayerQuest
    {
        public int PlayerId { get; set; }
        public Player? Player { get; set; }
        public int QuestId { get; set; }
        public Quest? Quest { get; set; }
        public DateTime AcceptedAt { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;
    }
}
