using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstagramDMs.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstagramBusinesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstagramPageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramBusinesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstagramBusinessMedia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOnServer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramBusinessMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramBusinessMedia_InstagramBusinesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "InstagramBusinesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramBusinessTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramBusinessTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramBusinessTemplates_InstagramBusinesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "InstagramBusinesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramContacts_InstagramBusinesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "InstagramBusinesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramTeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstagramBusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramTeamMembers_InstagramBusinesses_InstagramBusinessId",
                        column: x => x.InstagramBusinessId,
                        principalTable: "InstagramBusinesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramConversations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    LastMessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastMessageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMessageTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastReceivedMessageTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramConversations_InstagramBusinesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "InstagramBusinesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramConversations_InstagramContacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "InstagramContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramConversations_InstagramTeamMembers_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "InstagramTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramTeamMemberMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramTeamMemberMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramTeamMemberMessages_InstagramTeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "InstagramTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramConversationTimelineEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventSubtype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramConversationTimelineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramConversationTimelineEvents_InstagramConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "InstagramConversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramConversationTimelineEvents_InstagramTeamMembers_ActorId",
                        column: x => x.ActorId,
                        principalTable: "InstagramTeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsIncoming = table.Column<bool>(type: "bit", nullable: false),
                    ReadOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    TimelineEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramMessages_InstagramContacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "InstagramContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramMessages_InstagramConversationTimelineEvents_TimelineEventId",
                        column: x => x.TimelineEventId,
                        principalTable: "InstagramConversationTimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramMessages_InstagramConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "InstagramConversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstagramBusinessMedia_BusinessId",
                table: "InstagramBusinessMedia",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramBusinessTemplates_BusinessId",
                table: "InstagramBusinessTemplates",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramContacts_BusinessId",
                table: "InstagramContacts",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramConversations_AssignedToId",
                table: "InstagramConversations",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramConversations_BusinessId",
                table: "InstagramConversations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramConversations_ContactId",
                table: "InstagramConversations",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramConversationTimelineEvents_ActorId",
                table: "InstagramConversationTimelineEvents",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramConversationTimelineEvents_ConversationId",
                table: "InstagramConversationTimelineEvents",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramMessages_ContactId",
                table: "InstagramMessages",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramMessages_ConversationId",
                table: "InstagramMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramMessages_TimelineEventId",
                table: "InstagramMessages",
                column: "TimelineEventId",
                unique: true,
                filter: "[TimelineEventId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramTeamMemberMessages_TeamMemberId",
                table: "InstagramTeamMemberMessages",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramTeamMembers_InstagramBusinessId",
                table: "InstagramTeamMembers",
                column: "InstagramBusinessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstagramBusinessMedia");

            migrationBuilder.DropTable(
                name: "InstagramBusinessTemplates");

            migrationBuilder.DropTable(
                name: "InstagramMessages");

            migrationBuilder.DropTable(
                name: "InstagramTeamMemberMessages");

            migrationBuilder.DropTable(
                name: "InstagramConversationTimelineEvents");

            migrationBuilder.DropTable(
                name: "InstagramConversations");

            migrationBuilder.DropTable(
                name: "InstagramContacts");

            migrationBuilder.DropTable(
                name: "InstagramTeamMembers");

            migrationBuilder.DropTable(
                name: "InstagramBusinesses");
        }
    }
}
