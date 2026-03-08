using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoConfeitaria.Migrations
{
    /// <inheritdoc />
    public partial class CriaçãoColunasExclusao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "Vendas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "MateriaisUsados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExclusao",
                table: "Gastos",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "MateriaisUsados");

            migrationBuilder.DropColumn(
                name: "DataExclusao",
                table: "Gastos");
        }
    }
}
