using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionPlatformApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Membership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberInvitations_Users_InvitedByUserId",
                table: "MemberInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInvitations_Users_InvitedUserId",
                table: "MemberInvitations");

            migrationBuilder.DropIndex(
                name: "IX_MemberInvitations_InvitedByUserId",
                table: "MemberInvitations");

            migrationBuilder.DropIndex(
                name: "IX_MemberInvitations_InvitedUserId",
                table: "MemberInvitations");

            migrationBuilder.DropColumn(
                name: "InvitedByUserId",
                table: "MemberInvitations");

            migrationBuilder.DropColumn(
                name: "InvitedUserId",
                table: "MemberInvitations");

            migrationBuilder.AlterColumn<int>(
                name: "InvitationStatus",
                table: "MemberInvitations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InvitationStatus",
                table: "MemberInvitations",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "InvitedByUserId",
                table: "MemberInvitations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "InvitedUserId",
                table: "MemberInvitations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberInvitations_InvitedByUserId",
                table: "MemberInvitations",
                column: "InvitedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInvitations_InvitedUserId",
                table: "MemberInvitations",
                column: "InvitedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInvitations_Users_InvitedByUserId",
                table: "MemberInvitations",
                column: "InvitedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInvitations_Users_InvitedUserId",
                table: "MemberInvitations",
                column: "InvitedUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
