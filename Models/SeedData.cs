using Microsoft.EntityFrameworkCore;

namespace QuestBoard.Models
{
    public static class SeedData
    {
        public static async Task InitializeAsync(QuestBoardContext db)
        {
            await db.Database.EnsureCreatedAsync();

            if (!await db.Games.AnyAsync())
            {
                db.Games.AddRange(
                    new Game { Name = "Medieval Lands", Genre = "RPG", Description = "A fantasy realm.", CreatedAt = DateTime.UtcNow },
                    new Game { Name = "Arcane Trials", Genre = "Dungeon Crawler", Description = "Challenge dungeons.", CreatedAt = DateTime.UtcNow }
                );
                await db.SaveChangesAsync();
            }

            var firstGame = await db.Games.OrderBy(g => g.Id).FirstAsync();

            if (!await db.Players.AnyAsync())
            {
                db.Players.AddRange(
                    new Player { Name = "Aria", Handle = "aria01", Class = "Ranger", Level = 5, GameId = firstGame.Id },
                    new Player { Name = "Doran", Handle = "doranX", Class = "Warrior", Level = 7, GameId = firstGame.Id }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.Quests.AnyAsync())
            {
                db.Quests.AddRange(
                    new Quest
                    {
                        Title = "Slay 10 Slimes",
                        Name = "Slay 10 Slimes",
                        Description = "Training quest in the meadow.",
                        IsCompleted = false,
                        DueDateUtc = DateTime.UtcNow.AddDays(7),
                        GameId = firstGame.Id
                    },
                    new Quest
                    {
                        Title = "Deliver Herbs",
                        Name = "Deliver Herbs",
                        Description = "Bring 5 healing herbs to the village healer.",
                        IsCompleted = false,
                        DueDateUtc = DateTime.UtcNow.AddDays(3),
                        GameId = firstGame.Id
                    }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.PlayerQuests.AnyAsync())
            {
                var aria = await db.Players.OrderBy(p => p.Id).FirstAsync();
                var quest = await db.Quests.OrderBy(q => q.Id).FirstAsync();

                db.PlayerQuests.Add(new PlayerQuest
                {
                    PlayerId = aria.Id,
                    QuestId = quest.Id,
                    IsCompleted = false
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
