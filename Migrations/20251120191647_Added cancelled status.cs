using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kruggers_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Addedcancelledstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "CLOSED_CANCELLED" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
