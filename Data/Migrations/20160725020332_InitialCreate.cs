using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace passwordsafe3.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vault",
                columns: table => new
                {
                    VaultID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vault", x => x.VaultID);
                });

            migrationBuilder.CreateTable(
                name: "Password",
                columns: table => new
                {
                    PasswordID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Data = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    VaultID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Password", x => x.PasswordID);
                    table.ForeignKey(
                        name: "FK_Password_Vault_VaultID",
                        column: x => x.VaultID,
                        principalTable: "Vault",
                        principalColumn: "VaultID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaultUserKey",
                columns: table => new
                {
                    VaultID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Hash = table.Column<byte[]>(nullable: true),
                    MasterKey = table.Column<byte[]>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    VaultID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultUserKey", x => new { VaultID = x.VaultID, Username = x.Username });
                    table.ForeignKey(
                        name: "FK_VaultUserKey_Vault_VaultID1",
                        column: x => x.VaultID1,
                        principalTable: "Vault",
                        principalColumn: "VaultID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Password_VaultID",
                table: "Password",
                column: "VaultID");

            migrationBuilder.CreateIndex(
                name: "IX_VaultUserKey_VaultID1",
                table: "VaultUserKey",
                column: "VaultID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Password");

            migrationBuilder.DropTable(
                name: "VaultUserKey");

            migrationBuilder.DropTable(
                name: "Vault");
        }
    }
}
