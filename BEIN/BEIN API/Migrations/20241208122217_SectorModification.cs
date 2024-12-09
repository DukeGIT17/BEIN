using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BEIN_API.Migrations
{
    /// <inheritdoc />
    public partial class SectorModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SectorInformation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    SectorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorInformation_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Information = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SectorInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_SectorInformation_SectorInformationId",
                        column: x => x.SectorInformationId,
                        principalTable: "SectorInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectorPrinciple",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Principle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SectorInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorPrinciple", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorPrinciple_SectorInformation_SectorInformationId",
                        column: x => x.SectorInformationId,
                        principalTable: "SectorInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_Title",
                table: "Sectors",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_SectorInformationId",
                table: "CardInfo",
                column: "SectorInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorInformation_SectorId",
                table: "SectorInformation",
                column: "SectorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectorPrinciple_SectorInformationId",
                table: "SectorPrinciple",
                column: "SectorInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardInfo");

            migrationBuilder.DropTable(
                name: "SectorPrinciple");

            migrationBuilder.DropTable(
                name: "SectorInformation");

            migrationBuilder.DropIndex(
                name: "IX_Sectors_Title",
                table: "Sectors");
        }
    }
}
