namespace myBookSolution.Domain.Repositories.Token;

public interface ITokenUserRepository
{
    Task<Models.RefreshTokenUserModel?> Get(string refreshToken);
    Task SaveNewRefreshToken(Models.RefreshTokenUserModel refreshToken);
}
