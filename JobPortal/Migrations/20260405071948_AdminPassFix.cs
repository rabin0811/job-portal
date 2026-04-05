using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Migrations
{
    /// <inheritdoc />
    public partial class AdminPassFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$p6HQplJIKuOXbd4PAHAIgORUGGpNtok3GJ3q6MuC29EMnWa3155a2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$mC5bHnSInoMv9f9fX6XnGuF.rV/5L/mU0GkU/l9wT7S8n3y3pWfSy");
        }
    }
}
