using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJointTableForSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDay");

            migrationBuilder.DropTable(
                name: "ActivityProfile");

            migrationBuilder.CreateTable(
                name: "ActivityDays",
                columns: table => new
                {
                    ActivitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DaysBelongId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDays", x => new { x.DaysBelongId, x.ActivitiesId });
                    table.ForeignKey(
                        name: "FK_ActivityDays_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityDays_Days_DaysBelongId",
                        column: x => x.DaysBelongId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityProfiles",
                columns: table => new
                {
                    AssignedActivitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeachersAssignedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityProfiles", x => new { x.AssignedActivitiesId, x.TeachersAssignedId });
                    table.ForeignKey(
                        name: "FK_ActivityProfiles_Activities_AssignedActivitiesId",
                        column: x => x.AssignedActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityProfiles_Profiles_TeachersAssignedId",
                        column: x => x.TeachersAssignedId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDays_ActivitiesId",
                table: "ActivityDays",
                column: "ActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfiles_TeachersAssignedId",
                table: "ActivityProfiles",
                column: "TeachersAssignedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDays");

            migrationBuilder.DropTable(
                name: "ActivityProfiles");

            migrationBuilder.CreateTable(
                name: "ActivityDay",
                columns: table => new
                {
                    ActivitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DaysBelongId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDay", x => new { x.ActivitiesId, x.DaysBelongId });
                    table.ForeignKey(
                        name: "FK_ActivityDay_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityDay_Days_DaysBelongId",
                        column: x => x.DaysBelongId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityProfile",
                columns: table => new
                {
                    AssignedActivitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeachersAssignedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityProfile", x => new { x.AssignedActivitiesId, x.TeachersAssignedId });
                    table.ForeignKey(
                        name: "FK_ActivityProfile_Activities_AssignedActivitiesId",
                        column: x => x.AssignedActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityProfile_Profiles_TeachersAssignedId",
                        column: x => x.TeachersAssignedId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDay_DaysBelongId",
                table: "ActivityDay",
                column: "DaysBelongId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfile_TeachersAssignedId",
                table: "ActivityProfile",
                column: "TeachersAssignedId");
        }
    }
}
