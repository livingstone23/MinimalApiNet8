using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangoFinancialApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsurId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UsurId",
                table: "Comments",
                column: "UsurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UsurId",
                table: "Comments",
                column: "UsurId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UsurId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UsurId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UsurId",
                table: "Comments");
        }
    }
}
