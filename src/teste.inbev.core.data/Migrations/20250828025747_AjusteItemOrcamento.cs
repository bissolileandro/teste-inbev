using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace teste.inbev.core.data.Migrations
{
    /// <inheritdoc />
    public partial class AjusteItemOrcamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentualDesconto",
                table: "OrcamentoItem");

            migrationBuilder.DropColumn(
                name: "SubTotalItem",
                table: "OrcamentoItem");

            migrationBuilder.DropColumn(
                name: "ValorDesconto",
                table: "OrcamentoItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PercentualDesconto",
                table: "OrcamentoItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotalItem",
                table: "OrcamentoItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDesconto",
                table: "OrcamentoItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
