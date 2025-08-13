using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.Token;

public class TokenCuratorRepository : ITokenCuratorRepository
{
    private readonly MyDbContext _dbContext;

    public TokenCuratorRepository(MyDbContext context)
    {
        _dbContext = context;
    }

    public async Task<RefreshTokenCuratorModel?> Get(string refreshToken)
    {
        return await _dbContext
            .RefreshTokensCurators
            .AsNoTracking()
            .Include(token => token.Curator)
            .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    }

    public async Task SaveNewRefreshToken(RefreshTokenCuratorModel refreshToken)
    {
        var tokens = _dbContext.RefreshTokensCurators.Where(token => token.CuratorId == refreshToken.CuratorId);

        _dbContext.RefreshTokensCurators.RemoveRange(tokens);

        await _dbContext.RefreshTokensCurators.AddAsync(refreshToken);
    }
}
