using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeSharingApp.Migrations
{
    /// <inheritdoc />
    public partial class Adduserandpropertylistingassociation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HostID",
                table: "PropertyListings",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_HostID",
                table: "PropertyListings",
                column: "HostID");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyListings_AspNetUsers_HostID",
                table: "PropertyListings",
                column: "HostID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyListings_AspNetUsers_HostID",
                table: "PropertyListings");

            migrationBuilder.DropIndex(
                name: "IX_PropertyListings_HostID",
                table: "PropertyListings");

            migrationBuilder.AlterColumn<int>(
                name: "HostID",
                table: "PropertyListings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
