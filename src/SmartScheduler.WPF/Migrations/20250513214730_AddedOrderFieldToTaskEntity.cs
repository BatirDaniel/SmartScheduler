using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartScheduler.WPF.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderFieldToTaskEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedDate",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDate",
                table: "Tasks",
                type: "TEXT",
                nullable: true);
        }
    }
}
