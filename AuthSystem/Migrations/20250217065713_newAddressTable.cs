using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthSystem.Migrations
{
    /// <inheritdoc />
    public partial class newAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressModel_Users_UserId",
                table: "AddressModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressModel",
                table: "AddressModel");

            migrationBuilder.RenameTable(
                name: "AddressModel",
                newName: "Addressess");

            migrationBuilder.RenameIndex(
                name: "IX_AddressModel_UserId",
                table: "Addressess",
                newName: "IX_Addressess_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addressess",
                table: "Addressess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addressess_Users_UserId",
                table: "Addressess",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addressess_Users_UserId",
                table: "Addressess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addressess",
                table: "Addressess");

            migrationBuilder.RenameTable(
                name: "Addressess",
                newName: "AddressModel");

            migrationBuilder.RenameIndex(
                name: "IX_Addressess_UserId",
                table: "AddressModel",
                newName: "IX_AddressModel_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressModel",
                table: "AddressModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressModel_Users_UserId",
                table: "AddressModel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
