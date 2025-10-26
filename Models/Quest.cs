namespace QuestBoard.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RewardGold { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();
    }
}
