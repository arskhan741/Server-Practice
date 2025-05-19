using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsalanApp.Migrations
{
    /// <inheritdoc />
    public partial class nullabletest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "AuditEntries",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Changes",
                table: "AuditEntries",
                newName: "CurrentValues");

            migrationBuilder.AlterColumn<string>(
                name: "OldValues",
                table: "AuditEntries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EntityName",
                table: "AuditEntries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AuditEntries",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "CurrentValues",
                table: "AuditEntries",
                newName: "Changes");

            migrationBuilder.AlterColumn<string>(
                name: "OldValues",
                table: "AuditEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityName",
                table: "AuditEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
