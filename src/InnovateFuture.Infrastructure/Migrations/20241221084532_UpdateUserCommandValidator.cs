using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserCommandValidator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_invited_by",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_supervised_by",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Roles_role_id",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_user_id",
                table: "Profiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "default_profile",
                table: "Users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                table: "Profiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileProfile",
                columns: table => new
                {
                    InvitedProfilesProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupervisedProfilesProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileProfile", x => new { x.InvitedProfilesProfileId, x.SupervisedProfilesProfileId });
                    table.ForeignKey(
                        name: "FK_ProfileProfile_Profiles_InvitedProfilesProfileId",
                        column: x => x.InvitedProfilesProfileId,
                        principalTable: "Profiles",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileProfile_Profiles_SupervisedProfilesProfileId",
                        column: x => x.SupervisedProfilesProfileId,
                        principalTable: "Profiles",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_cognito_uuid",
                table: "Users",
                column: "cognito_uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RoleId1",
                table: "Profiles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileProfile_SupervisedProfilesProfileId",
                table: "ProfileProfile",
                column: "SupervisedProfilesProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_invited_by",
                table: "Profiles",
                column: "invited_by",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_supervised_by",
                table: "Profiles",
                column: "supervised_by",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Roles_RoleId1",
                table: "Profiles",
                column: "RoleId1",
                principalTable: "Roles",
                principalColumn: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Roles_role_id",
                table: "Profiles",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_invited_by",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_supervised_by",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Roles_RoleId1",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Roles_role_id",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "ProfileProfile");

            migrationBuilder.DropIndex(
                name: "IX_Users_cognito_uuid",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_RoleId1",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "Profiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "default_profile",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_user_id",
                table: "Profiles",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_invited_by",
                table: "Profiles",
                column: "invited_by",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_supervised_by",
                table: "Profiles",
                column: "supervised_by",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Roles_role_id",
                table: "Profiles",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
