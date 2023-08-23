using Microsoft.EntityFrameworkCore.Migrations;

namespace CommonLayer.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNo",
                table: "CardDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "CardDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "CardDetails",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UpiId",
                table: "CardDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNo",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "UpiId",
                table: "CardDetails");
        }
    }
}
