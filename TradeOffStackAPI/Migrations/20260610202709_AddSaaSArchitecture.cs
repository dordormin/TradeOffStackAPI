using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TradeOffStackAPI.Models;

#nullable disable

namespace TradeOffStackAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSaaSArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaaSProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    ContactEmail = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaSProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaaSSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanName = table.Column<string>(type: "text", nullable: false),
                    BillingCycle = table.Column<string>(type: "text", nullable: false),
                    CostPerSeat = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    RenewalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaSSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaaSSubscriptions_SaaSProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "SaaSProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaaSAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaSAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaaSAssignments_SaaSSubscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "SaaSSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaaSAssignments_SubscriptionId",
                table: "SaaSAssignments",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaSSubscriptions_ProviderId",
                table: "SaaSSubscriptions",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaaSAssignments");

            migrationBuilder.DropTable(
                name: "SaaSSubscriptions");

            migrationBuilder.DropTable(
                name: "SaaSProviders");
        }
    }
}
