using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HW78.Migrations
{
    /// <inheritdoc />
    public partial class add_data_time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id_category = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_category = table.Column<string>(type: "nchar(64)", fixedLength: true, maxLength: 64, nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id_category);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    id_expense = table.Column<int>(type: "int", nullable: false),
                    cost_expense = table.Column<double>(type: "float", nullable: false),
                    commentary = table.Column<string>(type: "text", nullable: false),
                    fk_category = table.Column<int>(type: "int", nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.id_expense);
                    table.ForeignKey(
                        name: "FK_Expence_Category",
                        column: x => x.id_expense,
                        principalTable: "Category",
                        principalColumn: "id_category");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
