using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NCRFiscalManager.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInitialtoFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicAuthUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicAuthUsers", x => x.Id);
                });
            
            migrationBuilder.CreateTable(
                name: "EmitterInVoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonType = table.Column<byte>(type: "tinyint", nullable: false),
                    RegimeType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IdentificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommercialRegistrationNum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ObligationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TributeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TributeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    IgnoresAllPriceZeroItems = table.Column<bool>(type: "bit", nullable: true),
                    UsesBlackList = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmitterInVoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechOperators",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechOperators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmitterInVoiceId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_EmitterInVoice_EmitterInVoiceId",
                        column: x => x.EmitterInVoiceId,
                        principalTable: "EmitterInVoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointOnSales",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmitterInVoiceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StoreKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InitInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinalInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsProduction = table.Column<bool>(type: "bit", nullable: false),
                    DateOfResolution = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Plazo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LlaveTecnica = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable:true),
                    ResolutionSerial = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOnSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointOnSales_EmitterInVoice_EmitterInVoiceId",
                        column: x => x.EmitterInVoiceId,
                        principalTable: "EmitterInVoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechOperatorEmitterInVoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmitterInVoiceId = table.Column<long>(type: "bigint", nullable: false),
                    TechOperatorId = table.Column<long>(type: "bigint", nullable: false),
                    ConnectionData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechOperatorEmitterInVoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechOperatorEmitterInVoice_EmitterInVoice_EmitterInVoiceId",
                        column: x => x.EmitterInVoiceId,
                        principalTable: "EmitterInVoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechOperatorEmitterInVoice_TechOperators_TechOperatorId",
                        column: x => x.TechOperatorId,
                        principalTable: "TechOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PointOnSaleId = table.Column<long>(type: "bigint", nullable: false),
                    CUFE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FiscalCorrelative = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocumentoFiscal = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceTransactions_PointOnSales_PointOnSaleId",
                        column: x => x.PointOnSaleId,
                        principalTable: "PointOnSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlackListedItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmitterInVoiceId = table.Column<long>(type: "bigint", nullable: false),
                    AlohaItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlohaItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackListedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackListedItems_EmitterInVoice_EmitterInVoiceId",
                        column: x => x.EmitterInVoiceId,
                        principalTable: "EmitterInVoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTransactions_PointOnSaleId",
                table: "InvoiceTransactions",
                column: "PointOnSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_EmitterInVoiceId",
                table: "PaymentMethods",
                column: "EmitterInVoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOnSales_EmitterInVoiceId",
                table: "PointOnSales",
                column: "EmitterInVoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TechOperatorEmitterInVoice_EmitterInVoiceId",
                table: "TechOperatorEmitterInVoice",
                column: "EmitterInVoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechOperatorEmitterInVoice_TechOperatorId",
                table: "TechOperatorEmitterInVoice",
                column: "TechOperatorId");

            migrationBuilder.CreateIndex(
               name: "IX_BlackListedItems_EmitterInVoiceId",
               table: "BlackListedItems",
               column: "EmitterInVoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTransactions");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "TechOperatorEmitterInVoice");

            migrationBuilder.DropTable(
                name: "PointOnSales");

            migrationBuilder.DropTable(
                name: "TechOperators");

            migrationBuilder.DropTable(
                name: "EmitterInVoice");

            migrationBuilder.DropTable(
                name: "BlackListedItems");
        }
    }
}
