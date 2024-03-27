using FairPlayCombined.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FairPlayShop.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SharedMigrations.Up(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            SharedMigrations.Down(migrationBuilder);
        }
    }
}
