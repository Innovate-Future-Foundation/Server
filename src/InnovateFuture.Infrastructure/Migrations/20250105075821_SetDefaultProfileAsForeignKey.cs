using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetDefaultProfileAsForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_default_profile",
                table: "Users",
                column: "default_profile",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profiles_default_profile",
                table: "Users",
                column: "default_profile",
                principalTable: "Profiles",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profiles_default_profile",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_default_profile",
                table: "Users");
        }
    }
}
