using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCore7API.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class FullName_Added_To_Token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Tokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Tokens");
        }
    }
}
