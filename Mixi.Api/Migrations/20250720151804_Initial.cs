using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mixi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdfDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    StorageStrategy = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    FormData = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "{}"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    UserType = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfDocument_CreatedAt",
                table: "PdfDocuments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PdfDocument_Name",
                table: "PdfDocuments",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfDocuments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
