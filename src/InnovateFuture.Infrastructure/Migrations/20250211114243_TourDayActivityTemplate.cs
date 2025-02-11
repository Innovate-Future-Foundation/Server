using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TourDayActivityTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourTemplates",
                columns: table => new
                {
                    TourTempId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganisationOrgId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplates", x => x.TourTempId);
                    table.ForeignKey(
                        name: "FK_TourTemplates_Organisations_OrganisationOrgId",
                        column: x => x.OrganisationOrgId,
                        principalTable: "Organisations",
                        principalColumn: "org_id");
                });

            migrationBuilder.CreateTable(
                name: "DayTemplates",
                columns: table => new
                {
                    DayTempId = table.Column<Guid>(type: "uuid", nullable: false),
                    TourTempId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTemplates", x => x.DayTempId);
                    table.ForeignKey(
                        name: "FK_DayTemplates_TourTemplates_TourTempId",
                        column: x => x.TourTempId,
                        principalTable: "TourTemplates",
                        principalColumn: "TourTempId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    TourId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganisationOrgId = table.Column<Guid>(type: "uuid", nullable: true),
                    TourTempId = table.Column<Guid>(type: "uuid", nullable: false),
                    TourLeadId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_Tours_Organisations_OrganisationOrgId",
                        column: x => x.OrganisationOrgId,
                        principalTable: "Organisations",
                        principalColumn: "org_id");
                    table.ForeignKey(
                        name: "FK_Tours_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "profile_id");
                    table.ForeignKey(
                        name: "FK_Tours_TourTemplates_TourTempId",
                        column: x => x.TourTempId,
                        principalTable: "TourTemplates",
                        principalColumn: "TourTempId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTemplates",
                columns: table => new
                {
                    ActivityTempId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayTempId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTemplates", x => x.ActivityTempId);
                    table.ForeignKey(
                        name: "FK_ActivityTemplates_DayTemplates_DayTempId",
                        column: x => x.DayTempId,
                        principalTable: "DayTemplates",
                        principalColumn: "DayTempId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    DayId = table.Column<Guid>(type: "uuid", nullable: false),
                    TourId = table.Column<Guid>(type: "uuid", nullable: true),
                    DayTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.DayId);
                    table.ForeignKey(
                        name: "FK_Days_DayTemplates_DayTemplateId",
                        column: x => x.DayTemplateId,
                        principalTable: "DayTemplates",
                        principalColumn: "DayTempId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Days_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActivityTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityTemplates_ActivityTemplateId",
                        column: x => x.ActivityTemplateId,
                        principalTable: "ActivityTemplates",
                        principalColumn: "ActivityTempId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "DayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTemplateId",
                table: "Activities",
                column: "ActivityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DayId",
                table: "Activities",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemplates_DayTempId",
                table: "ActivityTemplates",
                column: "DayTempId");

            migrationBuilder.CreateIndex(
                name: "IX_Days_DayTemplateId",
                table: "Days",
                column: "DayTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Days_TourId",
                table: "Days",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_DayTemplates_TourTempId",
                table: "DayTemplates",
                column: "TourTempId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_OrganisationOrgId",
                table: "Tours",
                column: "OrganisationOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_ProfileId",
                table: "Tours",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourTempId",
                table: "Tours",
                column: "TourTempId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplates_OrganisationOrgId",
                table: "TourTemplates",
                column: "OrganisationOrgId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ActivityTemplates");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "DayTemplates");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "TourTemplates");
        }
    }
}
