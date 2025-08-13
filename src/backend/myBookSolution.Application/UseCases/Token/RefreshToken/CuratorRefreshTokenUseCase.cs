using myBookSolution.API.Security.Tokens;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Domain.ValueObjects;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Token.RefreshToken;

public class CuratorRefreshTokenUseCase : ICuratorRefreshTokenUseCase
{
    private readonly ITokenCuratorRepository _tokenCuratorRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public CuratorRefreshTokenUseCase(
        IUnityOfWork unitOfWork,
        ITokenCuratorRepository tokenCuratorRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _unityOfWork = unitOfWork;
        _tokenCuratorRepository = tokenCuratorRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ResponseToken> Execute(RequestNewToken request)
    {
        var refreshToken = await _tokenCuratorRepository.Get(request.RefreshToken);

        if (refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(MyBookSolutionConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Models.RefreshTokenCuratorModel
        {
            Value = _refreshTokenGenerator.Generate(),
            CuratorId = refreshToken.CuratorId
        };

        await _tokenCuratorRepository.SaveNewRefreshToken(newRefreshToken);

        await _unityOfWork.Commit();

        return new ResponseToken
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.Curator.CuratorIdentifier, "curator"),
            RefreshToken = newRefreshToken.Value
        };
    }
}
