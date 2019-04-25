using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TamsPizzeriaWebApp.Migrations
{
    public partial class newMigration412 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Confirmation = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    FulFilledById = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    OnlineOrder = table.Column<bool>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    PizzaId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    StorePickupId = table.Column<int>(nullable: false),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StorePickupId",
                        column: x => x.StorePickupId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PizzaId",
                table: "Orders",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StorePickupId",
                table: "Orders",
                column: "StorePickupId");
        }
    }
}
