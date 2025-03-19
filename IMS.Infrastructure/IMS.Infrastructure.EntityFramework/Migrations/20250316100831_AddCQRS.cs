using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddCQRS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveredDate",
                table: "Orders",
                newName: "DeliveringDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveringDate",
                table: "Orders",
                newName: "DeliveredDate");
        }
    }
}
