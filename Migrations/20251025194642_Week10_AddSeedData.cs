using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuestBoard.Migrations
{
    /// <inheritdoc />
    public partial class Week10_AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CreatedAt", "Genre", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "RPG", "Medieval Lands" },
                    { 2, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Adventure", "Sky Realms" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Class", "GameId", "Level", "Name" },
                values: new object[,]
                {
                    { 1, "Mage", 1, 7, "Testwynd" },
                    { 2, "Knight", 1, 5, "Aeris" },
                    { 3, "Ranger", 2, 3, "Kael" }
                });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "GameId", "RewardGold", "Title" },
                values: new object[,]
                {
                    { 1, "Slay the troll in Frostpeak Cavern.", "Hard", 1, 250, "Defeat the Cave Troll" },
                    { 2, "Collect 10 Moonpetal flowers.", "Easy", 1, 100, "Gather Moonpetals" },
                    { 3, "Find gears to fix the skyship.", "Normal", 2, 180, "Skyship Repairs" },
                    { 4, "Explore the haunted floating isle.", "Hard", 2, 320, "Isle of Echoes" }
                });

            migrationBuilder.InsertData(
                table: "PlayerQuests",
                columns: new[] { "PlayerId", "QuestId", "AcceptedAt", "IsCompleted" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, 1, new DateTime(2025, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, 2, new DateTime(2025, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), true },
                    { 3, 3, new DateTime(2025, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), true },
                    { 3, 4, new DateTime(2025, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerQuests",
                keyColumns: new[] { "PlayerId", "QuestId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PlayerQuests",
                keyColumns: new[] { "PlayerId", "QuestId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "PlayerQuests",
                keyColumns: new[] { "PlayerId", "QuestId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PlayerQuests",
                keyColumns: new[] { "PlayerId", "QuestId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "PlayerQuests",
                keyColumns: new[] { "PlayerId", "QuestId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
