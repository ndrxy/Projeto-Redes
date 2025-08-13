using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace myBookSolution.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("Connection")!;
    }
}
