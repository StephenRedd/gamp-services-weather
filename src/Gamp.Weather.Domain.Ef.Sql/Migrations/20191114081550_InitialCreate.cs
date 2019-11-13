using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gamp.Weather.Domain.Ef.Sql.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gamp-weather");

            migrationBuilder.CreateTable(
                name: "Forecasts",
                schema: "gamp-weather",
                columns: table => new
                {
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    TemperatureC = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecasts", x => x.Date);
                });

            migrationBuilder.InsertData(
                schema: "gamp-weather",
                table: "Forecasts",
                columns: new[] { "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { new DateTimeOffset(new DateTime(2019, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), "Bracing", -7 },
                    { new DateTimeOffset(new DateTime(2019, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), "Hot", 30 },
                    { new DateTimeOffset(new DateTime(2019, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), "Bracing", 11 },
                    { new DateTimeOffset(new DateTime(2019, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), "Chilly", -16 },
                    { new DateTimeOffset(new DateTime(2019, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -8, 0, 0, 0)), "Hot", 53 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forecasts",
                schema: "gamp-weather");
        }
    }
}
