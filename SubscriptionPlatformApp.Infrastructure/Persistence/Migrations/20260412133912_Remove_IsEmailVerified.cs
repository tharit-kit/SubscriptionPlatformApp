using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionPlatformApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_IsEmailVerified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "EmailVerificationTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "EmailVerificationTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
