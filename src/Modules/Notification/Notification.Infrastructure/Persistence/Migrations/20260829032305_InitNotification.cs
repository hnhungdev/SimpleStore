using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "inbox_messages",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    received_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    processed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_error_message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_jobs",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    attempt_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    max_attempts = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    last_error_message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    send_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    processed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    claimed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    next_attempt_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    payload_body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payload_channel = table.Column<int>(type: "int", nullable: false),
                    payload_is_html = table.Column<bool>(type: "bit", nullable: false),
                    payload_recipient = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    payload_subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    payload_target_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    payload_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_templates",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    channel = table.Column<int>(type: "int", nullable: false),
                    subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_html = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_notifications",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    notification_type = table.Column<int>(type: "int", nullable: false),
                    is_read = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    read_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    target_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_notifications", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inbox_messages_processed_at",
                schema: "notification",
                table: "inbox_messages",
                column: "processed_at",
                filter: "[processed_at] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_notification_jobs_status_claimed_at_next_attempt_at",
                schema: "notification",
                table: "notification_jobs",
                columns: new[] { "status", "claimed_at", "next_attempt_at" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_jobs_status_priority",
                schema: "notification",
                table: "notification_jobs",
                columns: new[] { "status", "priority" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_templates_key",
                schema: "notification",
                table: "notification_templates",
                column: "key");

            migrationBuilder.CreateIndex(
                name: "IX_notification_templates_key_channel",
                schema: "notification",
                table: "notification_templates",
                columns: new[] { "key", "channel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_created_at",
                schema: "notification",
                table: "user_notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_user_id",
                schema: "notification",
                table: "user_notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_notifications_user_id_is_read",
                schema: "notification",
                table: "user_notifications",
                columns: new[] { "user_id", "is_read" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_jobs",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_templates",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "user_notifications",
                schema: "notification");
        }
    }
}
