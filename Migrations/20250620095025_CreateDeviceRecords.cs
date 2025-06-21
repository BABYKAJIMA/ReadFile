using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CameraView.Migrations
{
    /// <inheritdoc />
    public partial class CreateDeviceRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiKetNoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenThietBi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VLAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuRack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ODFPatchPannel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoSoiPort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNhanDan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ODF1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoSoi1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNhanDan2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ODF2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoSoi2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemDauCuoi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceRecords");
        }
    }
}
