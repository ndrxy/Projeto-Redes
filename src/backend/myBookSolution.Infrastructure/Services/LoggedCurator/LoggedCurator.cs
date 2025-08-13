using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Domain.Services.LoggedCurator;
using myBookSolution.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace myBookSolution.Infrastructure.Services.LoggedCurator;

public class LoggedCurator : ILoggedCurator
{
    private readonly MyDbContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoggedCurator(MyDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }

    public async Task<CuratorModel> CuratorLogged()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var curatorIdentifier = Guid.Parse(identifier);

        return await _context.Curators.AsNoTracking().FirstAsync(curator => curator.CuratorIdentifier == curatorIdentifier);
    }
}
