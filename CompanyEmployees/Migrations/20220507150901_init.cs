using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyEmployees.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compamies",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compamies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Compamies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Compamies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Compamies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("7b2e2490-0148-41a3-ba6a-6da08c699d7b"), "U kajdogo vtorogo", "Meste4kova9", "ShershaVa9 p9tka" });

            migrationBuilder.InsertData(
                table: "Compamies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"), "She9 andre9", "Liberty", "Dvoinoi podborodok" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("1196e3e2-feff-40d1-b88b-4074f3c90d1f"), 18, new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"), "Skoobi Doo", "Barista" },
                    { new Guid("a4ac1859-e888-4bc7-a94b-40814190b679"), 59, new Guid("7b2e2490-0148-41a3-ba6a-6da08c699d7b"), "Bazon Higgs", "World Developer" },
                    { new Guid("b69883db-64ee-467b-8e07-1ffd6c1545be"), 31, new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"), "Dora Nemo", "HR" },
                    { new Guid("e375886b-07ce-4771-86b5-3dc3a62b3e23"), 47, new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"), "Jack Jons", "Manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Compamies");
        }
    }
}
