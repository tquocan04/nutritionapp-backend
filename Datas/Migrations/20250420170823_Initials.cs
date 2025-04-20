using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datas.Migrations
{
    /// <inheritdoc />
    public partial class Initials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    TargetWeight = table.Column<float>(type: "real", nullable: true),
                    R = table.Column<float>(type: "real", nullable: true),
                    BMR = table.Column<float>(type: "real", nullable: true),
                    TDEE = table.Column<float>(type: "real", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalCalories = table.Column<float>(type: "real", nullable: false),
                    TotalCarbs = table.Column<float>(type: "real", nullable: false),
                    TotalFats = table.Column<float>(type: "real", nullable: false),
                    TotalProteins = table.Column<float>(type: "real", nullable: false),
                    TargetCalories = table.Column<float>(type: "real", nullable: false),
                    TargetCarbs = table.Column<float>(type: "real", nullable: false),
                    TargetFats = table.Column<float>(type: "real", nullable: false),
                    TargetProteins = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Breakfast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lunch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Dinner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeightTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightTrackings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Breakfasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalCalories = table.Column<float>(type: "real", nullable: false),
                    TotalCarbs = table.Column<float>(type: "real", nullable: false),
                    TotalFats = table.Column<float>(type: "real", nullable: false),
                    TotalProteins = table.Column<float>(type: "real", nullable: false),
                    DailyPlan_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breakfasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breakfasts_DailyPlans_DailyPlan_id",
                        column: x => x.DailyPlan_id,
                        principalTable: "DailyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dinners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalCalories = table.Column<float>(type: "real", nullable: false),
                    TotalCarbs = table.Column<float>(type: "real", nullable: false),
                    TotalFats = table.Column<float>(type: "real", nullable: false),
                    TotalProteins = table.Column<float>(type: "real", nullable: false),
                    DailyPlan_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dinners_DailyPlans_DailyPlan_id",
                        column: x => x.DailyPlan_id,
                        principalTable: "DailyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lunches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalCalories = table.Column<float>(type: "real", nullable: false),
                    TotalCarbs = table.Column<float>(type: "real", nullable: false),
                    TotalFats = table.Column<float>(type: "real", nullable: false),
                    TotalProteins = table.Column<float>(type: "real", nullable: false),
                    DailyPlan_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lunches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lunches_DailyPlans_DailyPlan_id",
                        column: x => x.DailyPlan_id,
                        principalTable: "DailyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemBreakfasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    Carb = table.Column<float>(type: "real", nullable: false),
                    Fat = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Breakfast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BreakfastId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBreakfasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemBreakfasts_Breakfasts_BreakfastId",
                        column: x => x.BreakfastId,
                        principalTable: "Breakfasts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemDinners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    Carb = table.Column<float>(type: "real", nullable: false),
                    Fat = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dinner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDinners_Dinners_DinnerId",
                        column: x => x.DinnerId,
                        principalTable: "Dinners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemLunches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    Carb = table.Column<float>(type: "real", nullable: false),
                    Fat = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lunch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LunchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLunches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemLunches_Lunches_LunchId",
                        column: x => x.LunchId,
                        principalTable: "Lunches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Breakfasts_DailyPlan_id",
                table: "Breakfasts",
                column: "DailyPlan_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyPlans_UserId",
                table: "DailyPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dinners_DailyPlan_id",
                table: "Dinners",
                column: "DailyPlan_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemBreakfasts_BreakfastId",
                table: "ItemBreakfasts",
                column: "BreakfastId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDinners_DinnerId",
                table: "ItemDinners",
                column: "DinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLunches_LunchId",
                table: "ItemLunches",
                column: "LunchId");

            migrationBuilder.CreateIndex(
                name: "IX_Lunches_DailyPlan_id",
                table: "Lunches",
                column: "DailyPlan_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeightTrackings_UserId",
                table: "WeightTrackings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemBreakfasts");

            migrationBuilder.DropTable(
                name: "ItemDinners");

            migrationBuilder.DropTable(
                name: "ItemLunches");

            migrationBuilder.DropTable(
                name: "WeightTrackings");

            migrationBuilder.DropTable(
                name: "Breakfasts");

            migrationBuilder.DropTable(
                name: "Dinners");

            migrationBuilder.DropTable(
                name: "Lunches");

            migrationBuilder.DropTable(
                name: "DailyPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
