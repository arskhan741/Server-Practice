using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArsalanApp.Migrations
{
    /// <inheritdoc />
    public partial class auditclassadded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditEntry",
                table: "AuditEntry");

            migrationBuilder.RenameTable(
                name: "AuditEntry",
                newName: "AuditEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditEntries",
                table: "AuditEntries",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditEntries",
                table: "AuditEntries");

            migrationBuilder.RenameTable(
                name: "AuditEntries",
                newName: "AuditEntry");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditEntry",
                table: "AuditEntry",
                column: "Id");
        }
    }
}
