using Microsoft.EntityFrameworkCore;

namespace QuestBoard.Models
{
    public class QuestBoardContext : DbContext
    {
        public QuestBoardContext(DbContextOptions<QuestBoardContext> options) : base(options) { }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Quest> Quests => Set<Quest>();
        public DbSet<PlayerQuest> PlayerQuests => Set<PlayerQuest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerQuest>().HasKey(pq => new { pq.PlayerId, pq.QuestId });

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Game)
                .WithMany(g => g.Players)
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quest>()
                .HasOne(q => q.Game)
                .WithMany(g => g.Quests)
                .HasForeignKey(q => q.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerQuest>()
                .HasOne(pq => pq.Player)
                .WithMany(p => p.PlayerQuests)
                .HasForeignKey(pq => pq.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerQuest>()
                .HasOne(pq => pq.Quest)
                .WithMany(q => q.PlayerQuests)
                .HasForeignKey(pq => pq.QuestId)
                .OnDelete(DeleteBehavior.Restrict);

            //AUTOMATICALLY SEED DATA 
           
            // Games
            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Name = "Elder Realms", Genre = "Fantasy RPG", CreatedAt = new DateTime(2025, 1, 1) },
                new Game { Id = 2, Name = "CyberStrike", Genre = "Sci-Fi Shooter", CreatedAt = new DateTime(2025, 2, 15) },
                new Game { Id = 3, Name = "Mythic Quest", Genre = "Adventure", CreatedAt = new DateTime(2025, 3, 10) }
            );

            // Players
            modelBuilder.Entity<Player>().HasData(
                new Player { Id = 1, Name = "Arin", Class = "Warrior", Level = 12, GameId = 1 },
                new Player { Id = 2, Name = "Lyra", Class = "Mage", Level = 15, GameId = 1 },
                new Player { Id = 3, Name = "Talon", Class = "Rogue", Level = 8, GameId = 2 },
                new Player { Id = 4, Name = "Vex", Class = "Sniper", Level = 10, GameId = 2 },
                new Player { Id = 5, Name = "Seren", Class = "Cleric", Level = 7, GameId = 3 }
            );

            // Quests
            modelBuilder.Entity<Quest>().HasData(
                new Quest { Id = 1, Title = "Defeat the Goblin King", Description = "Eliminate the Goblin King threatening Elder Realms.", RewardGold = 200, Difficulty = "Medium", GameId = 1 },
                new Quest { Id = 2, Title = "Recover the Lost Tome", Description = "Retrieve the ancient tome from the ruined library.", RewardGold = 150, Difficulty = "Easy", GameId = 1 },
                new Quest { Id = 3, Title = "Hack the Mainframe", Description = "Bypass corporate firewalls to steal the prototype.", RewardGold = 300, Difficulty = "Hard", GameId = 2 },
                new Quest { Id = 4, Title = "Eliminate the Rogue AI", Description = "Track and destroy the corrupted artificial intelligence.", RewardGold = 500, Difficulty = "Extreme", GameId = 2 },
                new Quest { Id = 5, Title = "Rescue the Village", Description = "Protect villagers from encroaching monsters.", RewardGold = 250, Difficulty = "Medium", GameId = 3 }
            );

            // PlayerQuests---
            modelBuilder.Entity<PlayerQuest>().HasData(
                new PlayerQuest { PlayerId = 1, QuestId = 1, AcceptedAt = new DateTime(2025, 4, 1), IsCompleted = true },
                new PlayerQuest { PlayerId = 2, QuestId = 2, AcceptedAt = new DateTime(2025, 4, 2), IsCompleted = false },
                new PlayerQuest { PlayerId = 3, QuestId = 3, AcceptedAt = new DateTime(2025, 4, 3), IsCompleted = false },
                new PlayerQuest { PlayerId = 4, QuestId = 4, AcceptedAt = new DateTime(2025, 4, 4), IsCompleted = true },
                new PlayerQuest { PlayerId = 5, QuestId = 5, AcceptedAt = new DateTime(2025, 4, 5), IsCompleted = false }
            );


        }
    }
}
