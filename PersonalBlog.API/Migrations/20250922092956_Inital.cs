using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalBlog.API.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UrlHandle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UrlHandle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    FeaturedImageUrl = table.Column<string>(type: "text", nullable: true),
                    UrlHandle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTags",
                columns: table => new
                {
                    BlogPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTags", x => new { x.BlogPostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogPostTags_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "UrlHandle" },
                values: new object[,]
                {
                    { new Guid("0e3fd9bd-0645-4809-8931-104b95245c04"), "Programming", "programming" },
                    { new Guid("752f16c7-4584-4e49-8896-d505057f04ba"), "Technology", "technology" },
                    { new Guid("a2d213cf-62c0-4127-b8da-77a7e4151a9c"), "Web Development", "web-development" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "UrlHandle" },
                values: new object[,]
                {
                    { new Guid("1f8b3dbd-b57c-4a02-b9e8-37a4daebce4c"), "API", "api" },
                    { new Guid("9b4d8833-cc77-4606-a0b8-f807873d65e3"), "C#", "csharp" },
                    { new Guid("9f1bf908-8c19-4072-9b22-bf463e39231d"), "Entity Framework", "entity-framework" },
                    { new Guid("bf1d9c57-8ea7-496a-9275-6dd53322296d"), "React", "react" },
                    { new Guid("c0a7938e-81cc-4d69-8f2f-b1ed8df3ac5a"), ".NET", "dotnet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CategoryId",
                table: "BlogPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_IsVisible",
                table: "BlogPosts",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PublishedDate",
                table: "BlogPosts",
                column: "PublishedDate");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UrlHandle",
                table: "BlogPosts",
                column: "UrlHandle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTags_TagId",
                table: "BlogPostTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UrlHandle",
                table: "Categories",
                column: "UrlHandle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UrlHandle",
                table: "Tags",
                column: "UrlHandle",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostTags");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
