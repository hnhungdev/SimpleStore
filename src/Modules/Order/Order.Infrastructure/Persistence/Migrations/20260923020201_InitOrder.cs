using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.CreateTable(
                name: "checkout_saga_state",
                schema: "order",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    current_state = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    payment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    failure_reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    payment_expires_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    completed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    reservation_timeout_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    payment_creation_timeout_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    payment_timeout_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkout_saga_state", x => x.correlation_id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_state",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lock_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    received = table.Column<DateTime>(type: "datetime2", nullable: false),
                    receive_count = table.Column<int>(type: "int", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    consumed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_state", x => x.id);
                    table.UniqueConstraint("AK_inbox_state_message_id_consumer_id", x => new { x.message_id, x.consumer_id });
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_no = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    order_status = table.Column<int>(type: "int", nullable: false),
                    payment_method = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    total_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    order_date = table.Column<DateOnly>(type: "date", nullable: false),
                    customer_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    customer_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    customer_phone_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_state",
                schema: "order",
                columns: table => new
                {
                    outbox_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lock_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_state", x => x.outbox_id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    product_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    product_thumbnail_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_histories",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    from_status = table.Column<int>(type: "int", nullable: false),
                    to_status = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_status_histories_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                schema: "order",
                columns: table => new
                {
                    sequence_number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    enqueue_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sent_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inbox_message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    inbox_consumer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    outbox_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content_type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    message_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    correlation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    initiator_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    source_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    destination_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    response_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    fault_address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    expiration_time = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_message", x => x.sequence_number);
                    table.ForeignKey(
                        name: "FK_outbox_message_inbox_state_inbox_message_id_inbox_consumer_id",
                        columns: x => new { x.inbox_message_id, x.inbox_consumer_id },
                        principalSchema: "order",
                        principalTable: "inbox_state",
                        principalColumns: new[] { "message_id", "consumer_id" });
                    table.ForeignKey(
                        name: "FK_outbox_message_outbox_state_outbox_id",
                        column: x => x.outbox_id,
                        principalSchema: "order",
                        principalTable: "outbox_state",
                        principalColumn: "outbox_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_checkout_saga_state_current_state",
                schema: "order",
                table: "checkout_saga_state",
                column: "current_state");

            migrationBuilder.CreateIndex(
                name: "IX_checkout_saga_state_order_id",
                schema: "order",
                table: "checkout_saga_state",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_inbox_state_delivered",
                schema: "order",
                table: "inbox_state",
                column: "delivered");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_order_id",
                schema: "order",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_histories_order_id",
                schema: "order",
                table: "order_status_histories",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_order_no",
                schema: "order",
                table: "orders",
                column: "order_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_order_status_created_at",
                schema: "order",
                table: "orders",
                columns: new[] { "order_status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_enqueue_time",
                schema: "order",
                table: "outbox_message",
                column: "enqueue_time");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_expiration_time",
                schema: "order",
                table: "outbox_message",
                column: "expiration_time");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_inbox_message_id_inbox_consumer_id_sequence_number",
                schema: "order",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id", "sequence_number" },
                unique: true,
                filter: "[inbox_message_id] IS NOT NULL AND [inbox_consumer_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_message_outbox_id_sequence_number",
                schema: "order",
                table: "outbox_message",
                columns: new[] { "outbox_id", "sequence_number" },
                unique: true,
                filter: "[outbox_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_state_created",
                schema: "order",
                table: "outbox_state",
                column: "created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checkout_saga_state",
                schema: "order");

            migrationBuilder.DropTable(
                name: "order_items",
                schema: "order");

            migrationBuilder.DropTable(
                name: "order_status_histories",
                schema: "order");

            migrationBuilder.DropTable(
                name: "outbox_message",
                schema: "order");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "order");

            migrationBuilder.DropTable(
                name: "inbox_state",
                schema: "order");

            migrationBuilder.DropTable(
                name: "outbox_state",
                schema: "order");
        }
    }
}
