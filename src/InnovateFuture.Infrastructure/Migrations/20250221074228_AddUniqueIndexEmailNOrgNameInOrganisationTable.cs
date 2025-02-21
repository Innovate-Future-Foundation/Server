using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexEmailNOrgNameInOrganisationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Organisations_Email",
                table: "Organisations",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_OrgName",
                table: "Organisations",
                column: "OrgName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organisations_Email",
                table: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Organisations_OrgName",
                table: "Organisations");
        }
    }
}
