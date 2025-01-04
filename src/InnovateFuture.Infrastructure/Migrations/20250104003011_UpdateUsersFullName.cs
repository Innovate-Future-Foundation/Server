using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "family_name",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "given_name",
                table: "Users",
                newName: "full_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Users",
                newName: "given_name");

            migrationBuilder.AddColumn<string>(
                name: "family_name",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
