using myBookSolution.Domain.Repositories;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess;

public class UnityOfWork : IUnityOfWork
{
    private readonly MyDbContext _dbContext;

    public UnityOfWork(MyDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
