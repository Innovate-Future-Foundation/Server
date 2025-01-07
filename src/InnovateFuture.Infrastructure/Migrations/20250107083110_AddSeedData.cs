using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Organisations",
                columns: new[] { "org_id", "address", "created_at", "email", "logo_url", "org_name", "status", "subscription", "updated_at", "website_url" },
                values: new object[,]
                {
                    { new Guid("0aecbf37-ead3-470c-ad8b-790d7eea3b0a"), null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1720), null, null, "org_name_02_test", (short)0, null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1720), null },
                    { new Guid("d96e643e-a7aa-42b0-a8cd-1cdd8610e857"), null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1720), null, null, "org_name_01_test", (short)0, null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1720), null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "role_id", "code_name", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("28c99a2a-e593-4353-8dc2-cb83fc1ebfea"), (short)4, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1630), "Access to tour details...", "Student", new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1630) },
                    { new Guid("32ef6536-3cb1-4846-bd32-cd34b489fd43"), (short)3, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1630), "Allows monitoring of a child’s progress...", "Parent", new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1630) },
                    { new Guid("3b69fda3-555a-4658-a6ab-31e1f327ef79"), (short)2, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1630), "Handles teaching-related responsibilities within the organisation, such as...", "Organisation Teacher", new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1630) },
                    { new Guid("d3788298-39b4-4a40-9985-bfa6a830acd9"), (short)1, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1620), "Oversees organisational-level operations, including...", "Organisation Admin", new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1620) },
                    { new Guid("e114c66a-07b2-4768-b0cf-c111895ce0c4"), (short)0, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1620), "Responsible for managing the entire platform, including...", "Platform Admin", new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1620) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "user_id", "birthday", "cognito_uuid", "created_at", "default_profile", "email", "full_name", "phone", "updated_at" },
                values: new object[] { new Guid("725f77b0-258a-4a92-827a-f5c4adfcba49"), null, new Guid("e95e0498-b0c1-700b-bb76-f571c5ec3f7c"), new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1750), null, "yangqingyan0@gmail.com", null, null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1750) });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "profile_id", "avatar", "created_at", "email", "invited_by", "is_active", "name", "org_id", "phone", "role_id", "supervised_by", "updated_at", "user_id" },
                values: new object[] { new Guid("4d69456b-9b86-43b9-b8f7-09a88062eb6b"), null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1770), null, null, true, null, new Guid("d96e643e-a7aa-42b0-a8cd-1cdd8610e857"), null, new Guid("e114c66a-07b2-4768-b0cf-c111895ce0c4"), null, new DateTime(2025, 1, 7, 8, 31, 10, 171, DateTimeKind.Utc).AddTicks(1770), new Guid("725f77b0-258a-4a92-827a-f5c4adfcba49") });
            
            migrationBuilder.Sql(
                @"UPDATE ""Users""
                SET default_profile = '4d69456b-9b86-43b9-b8f7-09a88062eb6b'
                WHERE user_id = '725f77b0-258a-4a92-827a-f5c4adfcba49';");
        }   

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Organisations",
                keyColumn: "org_id",
                keyValue: new Guid("0aecbf37-ead3-470c-ad8b-790d7eea3b0a"));

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "profile_id",
                keyValue: new Guid("4d69456b-9b86-43b9-b8f7-09a88062eb6b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "role_id",
                keyValue: new Guid("28c99a2a-e593-4353-8dc2-cb83fc1ebfea"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "role_id",
                keyValue: new Guid("32ef6536-3cb1-4846-bd32-cd34b489fd43"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "role_id",
                keyValue: new Guid("3b69fda3-555a-4658-a6ab-31e1f327ef79"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "role_id",
                keyValue: new Guid("d3788298-39b4-4a40-9985-bfa6a830acd9"));

            migrationBuilder.DeleteData(
                table: "Organisations",
                keyColumn: "org_id",
                keyValue: new Guid("d96e643e-a7aa-42b0-a8cd-1cdd8610e857"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "role_id",
                keyValue: new Guid("e114c66a-07b2-4768-b0cf-c111895ce0c4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: new Guid("725f77b0-258a-4a92-827a-f5c4adfcba49"));
        }
    }
}
