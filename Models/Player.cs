namespace QuestBoard.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int Level { get; set; }
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();
    }
}
