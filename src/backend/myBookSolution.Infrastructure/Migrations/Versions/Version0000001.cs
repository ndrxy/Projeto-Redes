using FluentMigrator;

namespace myBookSolution.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USERS, "Create table to save user's informations")]
public class Version0000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(100).NotNullable()
            .WithColumn("Password").AsString(250).NotNullable()
            .WithColumn("Cpf").AsString(11).NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable();
    }
}
