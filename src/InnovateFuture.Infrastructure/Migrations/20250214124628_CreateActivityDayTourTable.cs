using System;
using InnovateFuture.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateActivityDayTourTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:enrollment_status_enum", "undefined_enrollment,enrolled,dropped,completed")
                .Annotation("Npgsql:Enum:org_status_enum", "undefined_org_status,pending,active,suspended")
                .Annotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .Annotation("Npgsql:Enum:subscription_enum", "undefined_subscription,free,basic,premium")
                .Annotation("Npgsql:Enum:tour_status_enum", "undefined_tour,draft,published,active,updated,completed,canceled")
                .OldAnnotation("Npgsql:Enum:org_status_enum", "undefined_org_status,pending,active,suspended")
                .OldAnnotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .OldAnnotation("Npgsql:Enum:subscription_enum", "undefined_subscription,free,basic,premium");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Profiles",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Profiles",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Organisations",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Organisations",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CoverImgUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<TourStatusEnum>(type: "tour_status_enum", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Organisations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    CoverImgUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Status = table.Column<TourStatusEnum>(type: "tour_status_enum", nullable: false),
                    Leader = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tours_Organisations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tours_Profiles_Leader",
                        column: x => x.Leader,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    TourId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    CoverImgUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<TourStatusEnum>(type: "tour_status_enum", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Days_Organisations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Days_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentTourEnrollments",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TourId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    WithdrawalDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Status = table.Column<EnrollmentStatusEnum>(type: "enrollment_status_enum", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTourEnrollments", x => new { x.ProfileId, x.TourId });
                    table.ForeignKey(
                        name: "FK_StudentTourEnrollments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTourEnrollments_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OrgId",
                table: "Activities",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDay_DaysBelongId",
                table: "ActivityDay",
                column: "DaysBelongId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfile_TeachersAssignedId",
                table: "ActivityProfile",
                column: "TeachersAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Days_OrgId",
                table: "Days",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Days_TourId",
                table: "Days",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTourEnrollments_TourId",
                table: "StudentTourEnrollments",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_Leader",
                table: "Tours",
                column: "Leader");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_OrgId",
                table: "Tours",
                column: "OrgId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDay");

            migrationBuilder.DropTable(
                name: "ActivityProfile");

            migrationBuilder.DropTable(
                name: "StudentTourEnrollments");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:org_status_enum", "undefined_org_status,pending,active,suspended")
                .Annotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .Annotation("Npgsql:Enum:subscription_enum", "undefined_subscription,free,basic,premium")
                .OldAnnotation("Npgsql:Enum:enrollment_status_enum", "undefined_enrollment,enrolled,dropped,completed")
                .OldAnnotation("Npgsql:Enum:org_status_enum", "undefined_org_status,pending,active,suspended")
                .OldAnnotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .OldAnnotation("Npgsql:Enum:subscription_enum", "undefined_subscription,free,basic,premium")
                .OldAnnotation("Npgsql:Enum:tour_status_enum", "undefined_tour,draft,published,active,updated,completed,canceled");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Profiles",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Profiles",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Organisations",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Organisations",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");
        }
    }
}
