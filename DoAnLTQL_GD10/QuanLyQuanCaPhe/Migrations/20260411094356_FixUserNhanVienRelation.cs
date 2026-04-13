using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyQuanCaPhe.Migrations
{
    /// <inheritdoc />
    public partial class FixUserNhanVienRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_NhanVien_NhanVienId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NhanVien_NhanVienId",
                table: "Users",
                column: "NhanVienId",
                principalTable: "NhanVien",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[TR_NhanVien_Delete_BackupAudit]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[TR_NhanVien_Delete_BackupAudit];
");

            migrationBuilder.Sql(@"
CREATE TRIGGER [dbo].[TR_NhanVien_Delete_BackupAudit]
ON [dbo].[NhanVien]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DELETE u
    FROM [dbo].[Users] u
    INNER JOIN deleted d ON d.[ID] = u.[NhanVienId];

    IF OBJECT_ID(N'[dbo].[AuditLog]', N'U') IS NOT NULL
    BEGIN
        INSERT INTO [dbo].[AuditLog] ([Action], [EntityName], [EntityId], [OldValue], [NewValue], [PerformedBy], [CreatedAt])
        SELECT
            N'DeleteNhanVien_Trigger',
            N'NhanVien',
            CONVERT(nvarchar(50), d.[ID]),
            (
                SELECT
                    d.[ID] AS [NhanVienId],
                    d.[HoVaTen],
                    d.[DienThoai],
                    d.[DiaChi]
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ),
            (
                SELECT
                    CAST(1 AS bit) AS [DeletedByTrigger],
                    CAST(1 AS bit) AS [CascadeUserCleanup]
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ),
            N'db-trigger',
            SYSDATETIME()
        FROM deleted d;
    END
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[TR_NhanVien_Delete_BackupAudit]', N'TR') IS NOT NULL
    DROP TRIGGER [dbo].[TR_NhanVien_Delete_BackupAudit];
");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_NhanVien_NhanVienId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NhanVien_NhanVienId",
                table: "Users",
                column: "NhanVienId",
                principalTable: "NhanVien",
                principalColumn: "ID");
        }
    }
}
