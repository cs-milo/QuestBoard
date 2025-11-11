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
            modelBuilder.Entity<Game>().ToTable("Games");
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<Quest>().ToTable("Quests");
            modelBuilder.Entity<PlayerQuest>().ToTable("PlayerQuests");

            modelBuilder.Entity<PlayerQuest>()
                .HasKey(pq => new { pq.PlayerId, pq.QuestId });

            modelBuilder.Entity<PlayerQuest>()
                .HasOne(pq => pq.Player)
                .WithMany(p => p.PlayerQuests)
                .HasForeignKey(pq => pq.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerQuest>()
                .HasOne(pq => pq.Quest)
                .WithMany(q => q.PlayerQuests)
                .HasForeignKey(pq => pq.QuestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quest>()
                .HasOne(q => q.Game)
                .WithMany(g => g.Quests)
                .HasForeignKey(q => q.GameId)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
