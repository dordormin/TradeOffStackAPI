using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TradeOffStackAPI.Models;

#nullable disable

namespace TradeOffStackAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:asset_category", "None,Laptop,Monitor,Peripheral,NetworkDevice")
                .Annotation("Npgsql:Enum:asset_status", "None,Available,Reserved,OutForRepair,Retired")
                .Annotation("Npgsql:Enum:audit_action", "Created,Updated,Deleted")
                .Annotation("Npgsql:Enum:depreciation_method", "None,StraightLine,DecliningBalance")
                .Annotation("Npgsql:Enum:maintenance_priority", "Low,Medium,High,Critical")
                .Annotation("Npgsql:Enum:maintenance_status", "Pending,InProgress,Completed,Cancelled")
                .Annotation("Npgsql:Enum:reservation_status", "Pending,Approved,Rejected,Active,Returned,Cancelled");

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<AuditAction>(type: "audit_action", nullable: false),
                    OldValues = table.Column<string>(type: "jsonb", nullable: true),
                    NewValues = table.Column<string>(type: "jsonb", nullable: true),
                    PerformedById = table.Column<Guid>(type: "uuid", nullable: true),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "equipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<AssetStatus>(type: "asset_status", nullable: false),
                    Category = table.Column<AssetCategory>(type: "asset_category", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ImageUrlHttps = table.Column<string>(type: "text", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SalvageValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UsefulLifeYears = table.Column<int>(type: "integer", nullable: false),
                    DepreciationMethod = table.Column<DepreciationMethod>(type: "depreciation_method", nullable: false),
                    WarrantyExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipments", x => x.Id);
                });

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
                name: "software_licenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LicenseKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_licenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedById = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<MaintenanceStatus>(type: "maintenance_status", nullable: false),
                    Priority = table.Column<MaintenancePriority>(type: "maintenance_priority", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TechnicianNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_maintenance_requests_equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<ReservationStatus>(type: "reservation_status", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ApproverId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reservations_equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "equipment_licenses",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareLicenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_licenses", x => new { x.EquipmentId, x.SoftwareLicenseId });
                    table.ForeignKey(
                        name: "FK_equipment_licenses_equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_equipment_licenses_software_licenses_SoftwareLicenseId",
                        column: x => x.SoftwareLicenseId,
                        principalTable: "software_licenses",
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
                name: "IX_equipment_licenses_SoftwareLicenseId",
                table: "equipment_licenses",
                column: "SoftwareLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_requests_EquipmentId",
                table: "maintenance_requests",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_EquipmentId",
                table: "reservations",
                column: "EquipmentId");

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
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "equipment_licenses");

            migrationBuilder.DropTable(
                name: "maintenance_requests");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "SaaSAssignments");

            migrationBuilder.DropTable(
                name: "software_licenses");

            migrationBuilder.DropTable(
                name: "equipments");

            migrationBuilder.DropTable(
                name: "SaaSSubscriptions");

            migrationBuilder.DropTable(
                name: "SaaSProviders");
        }
    }
}
