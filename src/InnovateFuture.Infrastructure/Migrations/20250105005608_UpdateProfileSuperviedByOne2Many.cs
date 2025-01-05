using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfileSuperviedByOne2Many : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_supervised_by",
                table: "Profiles");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_supervised_by",
                table: "Profiles",
                column: "supervised_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_supervised_by",
                table: "Profiles");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_supervised_by",
                table: "Profiles",
                column: "supervised_by",
                unique: true);
        }
    }
}
