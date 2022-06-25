using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BadBroker.DataAccess.Psql.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyPairs",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    QuoteBase = table.Column<int>(type: "integer", nullable: false),
                    QuoteCurrency = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyPairs", x => new { x.Date, x.QuoteBase, x.QuoteCurrency });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyPairs");
        }
    }
}
