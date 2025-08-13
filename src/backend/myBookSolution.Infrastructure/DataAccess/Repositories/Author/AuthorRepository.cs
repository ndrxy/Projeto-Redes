using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.Author;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.Author;

public class AuthorRepository : IAuthorReadOnlyRepository, IAuthorWriteOnlyRepository
{
    private readonly MyDbContext _dbContext;

    public AuthorRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(AuthorModel author) => await _dbContext.Authors.AddAsync(author);

    public async Task<bool> AuthorAlreadyExists(string name) => await _dbContext.Authors.AnyAsync(author => author.Name.Equals(name));

    public async Task<long> GetAuthorIdByName(string name)
    {
        name = name.ToUpper();

        return await _dbContext.Authors
        .Where(a => a.Name == name)
        .Select(a => a.Id)
        .FirstOrDefaultAsync();

    }

    public async Task<IList<string>?> SearchByName(string name)
    {
        name = name.Trim().ToUpper();

        return await _dbContext.Authors.Select(a => a.Name).Take(5).ToListAsync();
    }

    
}
