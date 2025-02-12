using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IgnoreUnusedFieldsInUserTableCancelAlias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Organisations_org_id",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_inviter",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_supervisor",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Users_user_id",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "profile_id",
                table: "Users",
                newName: "DefaultProfileId");

            migrationBuilder.RenameColumn(
                name: "idp_subject",
                table: "Users",
                newName: "IdpSubject");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "supervisor",
                table: "Profiles",
                newName: "Supervisor");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Profiles",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Profiles",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Profiles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "inviter",
                table: "Profiles",
                newName: "Inviter");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Profiles",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Profiles",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Profiles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "org_id",
                table: "Profiles",
                newName: "OrgId");

            migrationBuilder.RenameColumn(
                name: "is_confirmed",
                table: "Profiles",
                newName: "IsConfirmed");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Profiles",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Profiles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "avatar_url",
                table: "Profiles",
                newName: "AvatarUrl");

            migrationBuilder.RenameColumn(
                name: "profile_id",
                table: "Profiles",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_supervisor",
                table: "Profiles",
                newName: "IX_Profiles_Supervisor");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_inviter",
                table: "Profiles",
                newName: "IX_Profiles_Inviter");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_user_id_role_id_org_id",
                table: "Profiles",
                newName: "IX_Profiles_user_id_role_org_id");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_org_id",
                table: "Profiles",
                newName: "IX_Profiles_OrgId");

            migrationBuilder.RenameColumn(
                name: "subscription",
                table: "Organisations",
                newName: "Subscription");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Organisations",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Organisations",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Organisations",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "website_url",
                table: "Organisations",
                newName: "WebsiteUrl");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Organisations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "org_name",
                table: "Organisations",
                newName: "OrgName");

            migrationBuilder.RenameColumn(
                name: "logo_url",
                table: "Organisations",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Organisations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "org_id",
                table: "Organisations",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Subscription",
                table: "Organisations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Organisations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultProfileId",
                table: "Users",
                column: "DefaultProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Organisations_OrgId",
                table: "Profiles",
                column: "OrgId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_Inviter",
                table: "Profiles",
                column: "Inviter",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Profiles_Supervisor",
                table: "Profiles",
                column: "Supervisor",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Users_UserId",
                table: "Profiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profiles_DefaultProfileId",
                table: "Users",
                column: "DefaultProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Organisations_OrgId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_Inviter",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Profiles_Supervisor",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Users_UserId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profiles_DefaultProfileId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DefaultProfileId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IdpSubject",
                table: "Users",
                newName: "idp_subject");

            migrationBuilder.RenameColumn(
                name: "DefaultProfileId",
                table: "Users",
                newName: "profile_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Supervisor",
                table: "Profiles",
                newName: "supervisor");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Profiles",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Profiles",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Profiles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Inviter",
                table: "Profiles",
                newName: "inviter");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Profiles",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Profiles",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Profiles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrgId",
                table: "Profiles",
                newName: "org_id");

            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Profiles",
                newName: "is_confirmed");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Profiles",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Profiles",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "Profiles",
                newName: "avatar_url");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Profiles",
                newName: "profile_id");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_Supervisor",
                table: "Profiles",
                newName: "IX_Profiles_supervisor");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_Inviter",
                table: "Profiles",
                newName: "IX_Profiles_inviter");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_user_id_role_org_id",
                table: "Profiles",
                newName: "IX_Profiles_user_id_role_id_org_id");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_OrgId",
                table: "Profiles",
                newName: "IX_Profiles_org_id");

            migrationBuilder.RenameColumn(
                name: "Subscription",
                table: "Organisations",
                newName: "subscription");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Organisations",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Organisations",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Organisations",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "WebsiteUrl",
                table: "Organisations",
                newName: "website_url");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Organisations",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrgName",
                table: "Organisations",
                newName: "org_name");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Organisations",
                newName: "logo_url");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Organisations",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Organisations",
                newName: "org_id");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "subscription",
                table: "Organisations",
                type: "integer",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<short>(
                name: "status",
                table: "Organisations",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Organisations_org_id",
                table: "Profiles",
                column: "org_id",
                principalTable: "Organisations",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Users_user_id",
                table: "Profiles",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
