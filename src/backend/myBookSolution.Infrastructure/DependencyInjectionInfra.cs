using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myBookSolution.API.Security.Tokens;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Author;
using myBookSolution.Domain.Repositories.Book;
using myBookSolution.Domain.Repositories.Borrowing;
using myBookSolution.Domain.Repositories.Curator;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Domain.Services.LoggedCurator;
using myBookSolution.Domain.Services.LoggedUser;
using myBookSolution.Infrastructure.Data;
using myBookSolution.Infrastructure.DataAccess;
using myBookSolution.Infrastructure.DataAccess.Repositories.Author;
using myBookSolution.Infrastructure.DataAccess.Repositories.Book;
using myBookSolution.Infrastructure.DataAccess.Repositories.Borrowing;
using myBookSolution.Infrastructure.DataAccess.Repositories.Curator;
using myBookSolution.Infrastructure.DataAccess.Repositories.Token;
using myBookSolution.Infrastructure.DataAccess.Repositories.User;
using myBookSolution.Infrastructure.Extensions;
using myBookSolution.Infrastructure.Security.Tokens.Access.Generator;
using myBookSolution.Infrastructure.Security.Tokens.Access.Validator;
using myBookSolution.Infrastructure.Security.Tokens.Refresh;
using myBookSolution.Infrastructure.Services.LoggedCurator;
using myBookSolution.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace myBookSolution.Infrastructure;

public static class DependencyInjectionInfra
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddTokens(services, configuration);
        AddDbContext(services, configuration);
        AddFluentMigrator(services, configuration);
        AddRepositories(services);
        AddLoggedUser(services);
        AddLoggedCurator(services);
    }

    public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        services.AddDbContext<MyDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString, b => b.MigrationsAssembly("myBookSolution.API"));
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnityOfWork, UnityOfWork>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

        services.AddScoped<ICuratorReadOnlyRepository, CuratorRepository>();
        services.AddScoped<ICuratorWriteOnlyRepository, CuratorRepository>();

        services.AddScoped<IAuthorReadOnlyRepository, AuthorRepository>();
        services.AddScoped<IAuthorWriteOnlyRepository, AuthorRepository>();

        services.AddScoped<IBookReadOnlyRepository, BookRepository>();
        services.AddScoped<IBookWriteOnlyRepository, BookRepository>();

        services.AddScoped<ITokenUserRepository, TokenUserRepository>();
        services.AddScoped<ITokenCuratorRepository, TokenCuratorRepository>();

        services.AddScoped<IBorrowingReadOnlyRepository, BorrowingRepository>();
        services.AddScoped<IBorrowingWriteOnlyRepository, BorrowingRepository>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("myBookSolution.Infrastructure")).For.All();

        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddLoggedCurator(IServiceCollection services) => services.AddScoped<ILoggedCurator, LoggedCurator>();
}
