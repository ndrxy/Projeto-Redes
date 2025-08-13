using FluentMigrator;

namespace myBookSolution.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_CURATORS, "Create table to save curator's informations")]
public class Version0000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Curators")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(100).NotNullable()
            .WithColumn("Password").AsString(250).NotNullable()
            .WithColumn("Cpf").AsString(11).NotNullable()
            .WithColumn("CuratorIdentifier").AsGuid().NotNullable();
    }
}
