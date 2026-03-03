using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TanLiHong_Project.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDateFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDateTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
