using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace myBookSolution.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        EnsureDatabaseCreated(connectionString);
        MigrationDatabase(serviceProvider);
    }

    private static void EnsureDatabaseCreated(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.InitialCatalog;

        // Conecta-se ao banco de dados 'master' para verificar a existência do banco
        connectionStringBuilder.InitialCatalog = "master";

        using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
        dbConnection.Open();

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @name", parameters);

        if (!records.Any())
        {
            // Se o banco de dados não existe, cria ele
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
        }
    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp();
    }
}
