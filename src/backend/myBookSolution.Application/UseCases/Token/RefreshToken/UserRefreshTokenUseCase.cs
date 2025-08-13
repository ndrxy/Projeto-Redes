using myBookSolution.API.Security.Tokens;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Domain.ValueObjects;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Token.RefreshToken;

public class UserRefreshTokenUseCase : IUserRefreshTokenUseCase
{
    private readonly ITokenUserRepository _tokenUserRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public UserRefreshTokenUseCase(
        IUnityOfWork unitOfWork,
        ITokenUserRepository tokenUserRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _unityOfWork = unitOfWork;
        _tokenUserRepository = tokenUserRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ResponseToken> Execute(RequestNewToken request)
    {
        var refreshToken = await _tokenUserRepository.Get(request.RefreshToken);

        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(MyBookSolutionConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Models.RefreshTokenUserModel
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _tokenUserRepository.SaveNewRefreshToken(newRefreshToken);

        await _unityOfWork.Commit();

        return new ResponseToken
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier, "user"),
            RefreshToken = newRefreshToken.Value
        };
    }
}
