using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnovateFuture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSubscriptionTypeInOrganisations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Organisations"" 
                ALTER COLUMN ""subscription"" TYPE SMALLINT 
                USING CASE 
                    WHEN ""subscription"" ~ '^[0-9]+$' THEN ""subscription""::smallint 
                    ELSE NULL 
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Organisations"" 
                ALTER COLUMN ""subscription"" TYPE VARCHAR(50) 
                USING ""subscription""::TEXT;
            ");
        }
    }
}
