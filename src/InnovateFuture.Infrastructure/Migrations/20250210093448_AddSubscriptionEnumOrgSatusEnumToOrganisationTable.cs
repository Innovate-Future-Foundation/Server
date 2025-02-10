using InnovateFuture.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionEnumOrgSatusEnumToOrganisationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "Subscription",
                table: "Organisations");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:org_status_enum", "undefined_org_status,pending,active,suspended")
                .Annotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .Annotation("Npgsql:Enum:subscription_enum", "undefined_subscription,free,basic,premium")
                .OldAnnotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student");

            migrationBuilder.AddColumn<SubscriptionEnum>(
                name: "Subscription",
                table: "Organisations",
                type: "subscription_enum",
                nullable: false,
                defaultValue: SubscriptionEnum.UndefinedSubscription);

            migrationBuilder.AddColumn<OrgStatusEnum>(
                name: "OrgStatus",
                table: "Organisations",
                type: "org_status_enum",
                nullable: false,
                defaultValue: OrgStatusEnum.UndefinedOrgStatus);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgStatus",
                table: "Organisations");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .OldAnnotation("Npgsql:Enum:org_status_enum", "undefined_org_status,pending,active,suspended")
                .OldAnnotation("Npgsql:Enum:role_enum", "undefined_role,platform_admin,org_admin,org_manager,org_teacher,parent,student")
                .OldAnnotation("Npgsql:Enum:subscription_enum", "undefined_subscription,free,basic,premium");

            migrationBuilder.AlterColumn<int>(
                name: "Subscription",
                table: "Organisations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(SubscriptionEnum),
                oldType: "subscription_enum");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Organisations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
