using FluentMigrator;

namespace myBookSolution.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_AUTHORS_BOOKS, "Create table to save curator's and book's informations")]
public class Version0000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Authors")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Description").AsString()
            .WithColumn("CuratorId").AsInt64().NotNullable().ForeignKey("FK_Author_Curator_Id", "Curators", "Id");

        Create.Table("Books")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Isbn").AsString(17)
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Description").AsString().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("CuratorId").AsInt64().NotNullable().ForeignKey("FK_Book_Curator_Id", "Curators", "Id")
            .WithColumn("AuthorId").AsInt64().NotNullable().ForeignKey("FK_Book_Author_Id", "Authors", "Id");
    }
}
