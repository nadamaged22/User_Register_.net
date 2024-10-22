using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePhotoUrlToAppUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a806aca-2d0d-4cdd-98d3-2ced27e31b9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d83904fe-5dba-429a-9c41-67fd4e99b6e2");

            migrationBuilder.AddColumn<string>(
                name: "publicId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8939c68e-b7b5-4d7d-a23e-5f181920e0f8", null, "Admin", "ADMIN" },
                    { "946f85f8-1db2-4c74-87f1-92670ec9692b", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8939c68e-b7b5-4d7d-a23e-5f181920e0f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "946f85f8-1db2-4c74-87f1-92670ec9692b");

            migrationBuilder.DropColumn(
                name: "publicId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a806aca-2d0d-4cdd-98d3-2ced27e31b9f", null, "User", "USER" },
                    { "d83904fe-5dba-429a-9c41-67fd4e99b6e2", null, "Admin", "ADMIN" }
                });
        }
    }
}
