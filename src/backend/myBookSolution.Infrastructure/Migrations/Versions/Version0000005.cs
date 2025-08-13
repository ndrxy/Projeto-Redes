using FluentMigrator;

namespace myBookSolution.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_BORROWINGS, "Create table to save borrowing's informations")]
public class Version0000005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Borrowings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("BorrowingDate").AsCustom("date").NotNullable()
            .WithColumn("CuratorId").AsInt64().ForeignKey("FK_Borrowing_Curator_Id", "Curators", "Id")
            .WithColumn("BookId").AsInt64().ForeignKey("FK_Borrowing_Book_Id", "Books", "Id")
            .WithColumn("UserId").AsInt64().ForeignKey("FK_Borrowing_User_Id", "Users", "Id")
            .WithColumn("Status").AsBoolean().NotNullable();
    }
}
