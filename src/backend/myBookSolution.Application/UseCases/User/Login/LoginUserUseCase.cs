using myBookSolution.API.Security.Tokens;
using myBookSolution.Application.Services.Cryptography;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.User.Login;

public class LoginUserUseCase : ILoginUserUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly PasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenUserRepository _tokenUserRepository;

    public LoginUserUseCase(IUserReadOnlyRepository repository,
        IAccessTokenGenerator accessTokenGenerator,
        PasswordEncrypter passwordEncrypter,
        IUnityOfWork unityOfWork,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenUserRepository tokenUserRepository)
    {
        _repository = repository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncrypter = passwordEncrypter;
        _unityOfWork = unityOfWork;
        _tokenUserRepository = tokenUserRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestUserLogin req)
    {
        var encryptedPassword = _passwordEncrypter.Encrypt(req.Password);

        var user = await _repository.GetByEmailAndPassword(req.Email, encryptedPassword) ?? throw new InvalidLoginException();

        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisteredUser
        {
            Name = user.Name,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, "user"), //adicionado string
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Models.UserModel user)
    {
        var refreshToken = new Domain.Models.RefreshTokenUserModel
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = user.Id
        };

        await _tokenUserRepository.SaveNewRefreshToken(refreshToken);

        await _unityOfWork.Commit();

        return refreshToken.Value;
    }
}
