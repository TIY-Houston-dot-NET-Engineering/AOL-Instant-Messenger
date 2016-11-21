using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace aolClone.Migrations
{
    public partial class updateRelations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rooms_RoomId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AOLUsers",
                table: "Users",
                column: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Messages",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Messages",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AOLUsers_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AOLUsers_Rooms_RoomId",
                table: "Users",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoomId",
                table: "Users",
                newName: "IX_AOLUsers_RoomId");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AOLUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AOLUsers_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_AOLUsers_Rooms_RoomId",
                table: "AOLUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AOLUsers",
                table: "AOLUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "AOLUsers",
                column: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AOLUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rooms_RoomId",
                table: "AOLUsers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_AOLUsers_RoomId",
                table: "AOLUsers",
                newName: "IX_Users_RoomId");

            migrationBuilder.RenameTable(
                name: "AOLUsers",
                newName: "Users");
        }
    }
}
