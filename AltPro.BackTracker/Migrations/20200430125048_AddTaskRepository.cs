using Microsoft.EntityFrameworkCore.Migrations;

namespace AltPro.BackTracker.Migrations
{
    public partial class AddTaskRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskModels",
                columns: table => new
                {
                    TaskModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(nullable: false),
                    ModuleName = table.Column<string>(nullable: false),
                    ReporterID = table.Column<string>(nullable: false),
                    AssignedID = table.Column<string>(nullable: true),
                    TaskPriority = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    TaskState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModels", x => x.TaskModelId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskModels");
        }
    }
}
