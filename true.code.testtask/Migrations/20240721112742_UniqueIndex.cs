using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace @true.code.testtask.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Priorities_Level",
                table: "Priorities",
                column: "Level",
                unique: true);

            migrationBuilder.Sql(@"insert into ""Priorities""(""Level"") values (1),(2),(3),(4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Priorities_Level",
                table: "Priorities");
        }
    }
}
