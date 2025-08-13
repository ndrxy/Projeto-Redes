using myBookSolution.API.Security.Tokens;
using myBookSolution.Application.Services.Cryptography;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Curator;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Curator.Login;

public class LoginCuratorUseCase : ILoginCuratorUseCase
{
    private readonly ICuratorReadOnlyRepository _repository;
    private readonly PasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenCuratorRepository _tokenCuratorRepository;

    public LoginCuratorUseCase(ICuratorReadOnlyRepository repository,
        IAccessTokenGenerator accessTokenGenerator,
        PasswordEncrypter passwordEncrypter,
        IUnityOfWork unityOfWork,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenCuratorRepository tokenCuratorRepository)
    {
        _repository = repository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncrypter = passwordEncrypter;
        _unityOfWork = unityOfWork;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenCuratorRepository = tokenCuratorRepository;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestUserLogin req)
    {
        var encryptedPassword = _passwordEncrypter.Encrypt(req.Password);

        var curator = await _repository.GetByEmailAndPassword(req.Email, encryptedPassword) ?? throw new InvalidLoginException();

        var refreshToken = await CreateAndSaveRefreshToken(curator);

        return new ResponseRegisteredUser
        {
            Name = curator.Name,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(curator.CuratorIdentifier, "curator"), //adicionado string
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Models.CuratorModel curator)
    {
        var refreshToken = new Domain.Models.RefreshTokenCuratorModel
        {
            Value = _refreshTokenGenerator.Generate(),
            CuratorId = curator.Id
        };

        await _tokenCuratorRepository.SaveNewRefreshToken(refreshToken);

        await _unityOfWork.Commit();

        return refreshToken.Value;
    }

}
