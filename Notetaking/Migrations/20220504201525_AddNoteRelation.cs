using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notetaking.Migrations
{
    public partial class AddNoteRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoteRelation",
                columns: table => new
                {
                    FromNoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToNoteId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteRelation", x => new { x.FromNoteId, x.ToNoteId });
                    table.ForeignKey(
                        name: "FK_NoteRelation_Notes_FromNoteId",
                        column: x => x.FromNoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteRelation_Notes_ToNoteId",
                        column: x => x.ToNoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteRelation_ToNoteId",
                table: "NoteRelation",
                column: "ToNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteRelation");
        }
    }
}
