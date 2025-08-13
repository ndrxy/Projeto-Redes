using FluentMigrator;

namespace myBookSolution.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Create table to save curator's and book's informations")]
public class Version0000004 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("RefreshTokensUsers")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedOn").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Value").AsString().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id");

        Create.Table("RefreshTokensCurators")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedOn").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Value").AsString().NotNullable()
            .WithColumn("CuratorId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_Curator_Id", "Curators", "Id");
    }
}
