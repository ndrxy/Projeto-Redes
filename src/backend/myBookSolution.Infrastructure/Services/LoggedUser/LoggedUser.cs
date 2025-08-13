using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Domain.Services.LoggedUser;
using myBookSolution.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace myBookSolution.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly MyDbContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(MyDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }

    public async Task<UserModel> UserLogged()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _context.Users.AsNoTracking().FirstAsync(user => user.UserIdentifier == userIdentifier);
    }
}
