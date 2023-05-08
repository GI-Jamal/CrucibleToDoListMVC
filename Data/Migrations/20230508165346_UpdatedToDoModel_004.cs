using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrucibleToDoListMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedToDoModel_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_AspNetUsers_AppUserId",
                table: "ToDoItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ToDoItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "ToDoItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_AspNetUsers_AppUserId",
                table: "ToDoItems",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_AspNetUsers_AppUserId",
                table: "ToDoItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ToDoItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "ToDoItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_AspNetUsers_AppUserId",
                table: "ToDoItems",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
