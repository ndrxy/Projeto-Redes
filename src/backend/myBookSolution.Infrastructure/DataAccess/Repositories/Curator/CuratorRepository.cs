using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.Curator;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.Curator;

public class CuratorRepository : ICuratorReadOnlyRepository, ICuratorWriteOnlyRepository
{
    private readonly MyDbContext _dbContext;

    public CuratorRepository(MyDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(CuratorModel curator) => await _dbContext.Curators.AddAsync(curator);

    public async Task<bool> ExistsActiveCuratorWithCpf(string cpf) => await _dbContext.Curators.AnyAsync(curator => curator.Cpf == cpf);

    public async Task<bool> ExistsActiveCuratorWithEmail(string email) => await _dbContext.Curators.AnyAsync(curator => curator.Email.Equals(email));

    public async Task<bool> ExistsActiveCuratorWithIdentifier(Guid curatorIdentifier) => await _dbContext.Curators.AnyAsync(curator => curator.CuratorIdentifier.Equals(curatorIdentifier));

    public async Task<CuratorModel?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext
            .Curators
            .AsNoTracking()
            .FirstOrDefaultAsync(curator => curator.Email.Equals(email) && curator.Password.Equals(password));
    }


}
