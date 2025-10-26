namespace QuestBoard.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Quest> Quests { get; set; } = new List<Quest>();
    }
}
