using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mixi.Api.Migrations
{
    /// <inheritdoc />
    public partial class MixiDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfDocuments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdfDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FormData = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "{}"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StorageStrategy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfDocument_CreatedAt",
                table: "PdfDocuments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PdfDocument_Name",
                table: "PdfDocuments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PdfDocuments_UserId",
                table: "PdfDocuments",
                column: "UserId");
        }
    }
}
