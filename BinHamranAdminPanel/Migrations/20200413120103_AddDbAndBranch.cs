using Microsoft.EntityFrameworkCore.Migrations;

namespace BinHamranAdminPanel.Migrations
{
    public partial class AddDbAndBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbNames",
                columns: table => new
                {
                    DatabaseNameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DbArabicName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DbEnglishName = table.Column<string>(nullable: true),
                    DbName = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbNames", x => x.DatabaseNameId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DbBranchID = table.Column<int>(nullable: false),
                    BRN_AR_NAME = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BRN_EN_NAME = table.Column<string>(nullable: true),
                    DatabaseNameId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_DbNames_DatabaseNameId",
                        column: x => x.DatabaseNameId,
                        principalTable: "DbNames",
                        principalColumn: "DatabaseNameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_DatabaseNameId",
                table: "Branches",
                column: "DatabaseNameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "DbNames");
        }
    }
}
