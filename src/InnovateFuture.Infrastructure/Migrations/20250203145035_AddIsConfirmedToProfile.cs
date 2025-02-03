using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsConfirmedToProfile : Migration
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

            migrationBuilder.RenameColumn(
                name: "supervised_by",
                table: "Profiles",
                newName: "supervisor");

            migrationBuilder.RenameColumn(
                name: "invited_by",
                table: "Profiles",
                newName: "inviter");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_supervised_by",
                table: "Profiles",
                newName: "IX_Profiles_supervisor");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_invited_by",
                table: "Profiles",
                newName: "IX_Profiles_inviter");

            migrationBuilder.AddColumn<bool>(
                name: "is_confirmed",
                table: "Profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_inviter",
                table: "Profiles",
                column: "inviter",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_supervisor",
                table: "Profiles",
                column: "supervisor",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_inviter",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_supervisor",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "is_confirmed",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "supervisor",
                table: "Profiles",
                newName: "supervised_by");

            migrationBuilder.RenameColumn(
                name: "inviter",
                table: "Profiles",
                newName: "invited_by");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_supervisor",
                table: "Profiles",
                newName: "IX_Profiles_supervised_by");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_inviter",
                table: "Profiles",
                newName: "IX_Profiles_invited_by");

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
        }
    }
}
