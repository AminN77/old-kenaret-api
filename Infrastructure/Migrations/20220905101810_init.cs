using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    LinkId = table.Column<Guid>(type: "uuid", nullable: false),
                    LiveStreamId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => new { x.LinkId, x.LiveStreamId });
                });

            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LiveStreamId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsConnected = table.Column<bool>(type: "boolean", nullable: false),
                    Permission = table.Column<int>(type: "integer", nullable: false),
                    LastStatusChangeDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalDuration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => new { x.UserId, x.LiveStreamId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRegisterCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenExpireTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "World",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_World", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiveStreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    MainQuestion = table.Column<string>(type: "text", nullable: false),
                    StreamerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorldId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false),
                    GuestsCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveStreams_Users_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiveStreams_World_WorldId",
                        column: x => x.WorldId,
                        principalTable: "World",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "CreateDateTime", "FirstName", "IsAdmin", "IsRegisterCompleted", "LastLoginDateTime", "LastName", "PhoneNumber", "RefreshToken", "RefreshTokenExpireTime", "Username" },
                values: new object[] { new Guid("4f590a70-94ce-4d15-9494-5248280c2ce3"), "im am here", new DateTime(2022, 9, 5, 10, 18, 10, 581, DateTimeKind.Utc).AddTicks(6670), "mediasoup", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "server", "09009009000", "qkjbcoi238yehasd", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin_mediasoup_server" });

            migrationBuilder.InsertData(
                table: "World",
                columns: new[] { "Id", "CreateDateTime", "Title" },
                values: new object[,]
                {
                    { new Guid("07b2f146-b624-4c8e-bdf0-e5172c5e9f1c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "فرهنگ، هنر، معماری" },
                    { new Guid("0f3ac0a6-655e-4498-8590-eaabd5788e9a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "مشاوره و روانشناسی" },
                    { new Guid("10fd49fd-2187-406a-830c-b367035cd448"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سیاست" },
                    { new Guid("17d0f3ef-6ff3-4868-b924-c13c5c45ab49"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ورزشی، مد، زیبایی و تغذیه" },
                    { new Guid("2cb6f432-2472-4681-8f22-4c4a94853f0d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "بیمه، مالیات، شرکت" },
                    { new Guid("4bf0d2a9-b62b-4f58-a18b-a1eef5c3b9c7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "تفریح، سرگرمی، سفر" },
                    { new Guid("6018a1df-ad09-4005-8c36-29db689fc7e6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "اقتصاد، بورس، مالی، سرمایه گذاری" },
                    { new Guid("7638e5c6-cba1-4071-9a0b-ecff4d9c645d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سلامتی و پزشکی" },
                    { new Guid("8f00ea20-aec9-4051-ba2c-9397ee406e63"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "نظلم وظیفه و اداری" },
                    { new Guid("c0f3220a-d688-4535-ae72-feabe2e61f2c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "سرمایه انسانی و توسعه فردی" },
                    { new Guid("c5df6845-f8c0-4062-9ebd-7079a5a5f615"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "مهاجرت و ادامه ی تحصیل" },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "استارت آپ و کسب و کار" },
                    { new Guid("e1c1ed84-b379-4c24-9121-c1c6263491b3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "فلسفه، دین، مذهب" },
                    { new Guid("e8bdbbdf-4624-4501-9c39-505c60316c1b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "حقوقی، قضایی، وکالت" },
                    { new Guid("edf6a3f8-c2f1-454d-85e9-11eab8fad28f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "تکنولوژی و مهندسی" },
                    { new Guid("ee19c0cc-ed03-4c6b-84cd-a1e5f31ecada"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "خرید و فروش" },
                    { new Guid("f68eb329-64f6-425c-9052-f0cf42117524"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "آموزشی" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveStreams_StreamerId",
                table: "LiveStreams",
                column: "StreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveStreams_WorldId",
                table: "LiveStreams",
                column: "WorldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "LiveStreams");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "World");
        }
    }
}
