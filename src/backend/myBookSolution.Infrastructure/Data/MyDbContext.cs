using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;

namespace myBookSolution.Infrastructure.Data;

public class MyDbContext : DbContext
{
    public DbSet<UserModel>Users { get; set; }
    public DbSet<AuthorModel>Authors { get; set; }
    public DbSet<BookModel>Books { get; set; }
    public DbSet<CuratorModel>Curators { get; set; }
    public DbSet<BorrowingModel> Borrowings { get; set; }
    public DbSet<RefreshTokenUserModel> RefreshTokensUsers { get; set; }
    public DbSet<RefreshTokenCuratorModel> RefreshTokensCurators { get; set; }

    public MyDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
    }
}
