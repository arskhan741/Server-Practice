using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsalanApp.Migrations
{
    /// <inheritdoc />
    public partial class AuditClassoldvaluestest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "AuditEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "AuditEntries");
        }
    }
}
