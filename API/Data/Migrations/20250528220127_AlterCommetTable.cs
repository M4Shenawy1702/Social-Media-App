using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.data.migrations
{
    /// <inheritdoc />
    public partial class AlterCommetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "PostComments");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostComments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_UserId",
                table: "PostComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Users_UserId",
                table: "PostComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Users_UserId",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_UserId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostComments");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "PostComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
