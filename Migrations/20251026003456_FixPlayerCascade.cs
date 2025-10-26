using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuestBoard.Migrations
{
    /// <inheritdoc />
    public partial class FixPlayerCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerQuests_Players_PlayerId",
                table: "PlayerQuests");

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

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerQuests_Players_PlayerId",
                table: "PlayerQuests",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerQuests_Players_PlayerId",
                table: "PlayerQuests");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Quests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quests",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Players",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "Players",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Games",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerQuests_Players_PlayerId",
                table: "PlayerQuests",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
