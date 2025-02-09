using System;
using InnovateFuture.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleTableUpdateRoleColumnToRoleEnumType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Roles_role_id",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_role_id",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_user_id_role_id_org_id",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "Profiles");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student");

            migrationBuilder.AddColumn<RoleEnum>(
                name: "role",
                table: "Profiles",
                type: "role_enum",
                nullable: false,
                defaultValue: RoleEnum.UndefinedRole);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_user_id_role_org_id",
                table: "Profiles",
                columns: new[] { "user_id", "role", "org_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_user_id_role_org_id",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "role",
                table: "Profiles");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student");

            migrationBuilder.AddColumn<Guid>(
                name: "role_id",
                table: "Profiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code_name = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.role_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_role_id",
                table: "Profiles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_user_id_role_id_org_id",
                table: "Profiles",
                columns: new[] { "user_id", "role_id", "org_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_code_name",
                table: "Roles",
                column: "code_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_name",
                table: "Roles",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Roles_role_id",
                table: "Profiles",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
