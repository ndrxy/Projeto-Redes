using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.User;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly MyDbContext _dbContext;

    public UserRepository(MyDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(UserModel user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistsActiveUserWithCpf(string cpf) => await _dbContext.Users.AnyAsync(user => user.Cpf == cpf);

    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));

    public async Task<bool> ExistsActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => userIdentifier.Equals(userIdentifier));

    public async Task<UserModel?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password));
    }

    public async Task<long> GetIdByCpf(string cpf)
    {
        return await _dbContext.Users
        .Where(u => u.Cpf == cpf)
        .Select(u => u.Id)
        .FirstOrDefaultAsync();
    }

    public async Task<UserModel?> GetUserByCpf(string cpf)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Cpf.Equals(cpf));
    }
}
