using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.Token;

public class TokenUserRepository : ITokenUserRepository
{
    private readonly MyDbContext _dbContext;

    public TokenUserRepository(MyDbContext context)
    {
        _dbContext = context;
    }

    public async Task<RefreshTokenUserModel?> Get(string refreshToken)
    {
        return await _dbContext
            .RefreshTokensUsers
            .AsNoTracking()
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    }

    public async Task SaveNewRefreshToken(RefreshTokenUserModel refreshToken)
    {
        var tokens = _dbContext.RefreshTokensUsers.Where(token => token.UserId == refreshToken.UserId);

        _dbContext.RefreshTokensUsers.RemoveRange(tokens);

        await _dbContext.RefreshTokensUsers.AddAsync(refreshToken);
    }
}
