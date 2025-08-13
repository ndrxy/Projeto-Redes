namespace myBookSolution.Domain.Repositories.Token;

public interface ITokenCuratorRepository
{
    Task<Models.RefreshTokenCuratorModel?> Get(string refreshToken);
    Task SaveNewRefreshToken(Models.RefreshTokenCuratorModel refreshToken);
}