USE QuestBoardDb;
GO

CREATE OR ALTER PROCEDURE dbo.GetOpenQuestsForGame
    @GameId int
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        q.Id,
        q.Title,
        q.Description,
        q.RewardGold,
        q.Difficulty,
        q.GameId
    FROM Quests AS q
    WHERE q.GameId = @GameId
    ORDER BY q.Difficulty, q.Id;
END;
GO
