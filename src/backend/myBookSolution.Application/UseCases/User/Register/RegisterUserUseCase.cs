using AutoMapper;
using myBookSolution.API.Security.Tokens;
using myBookSolution.Application.Services.CpfValidator;
using myBookSolution.Application.Services.Cryptography;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IMapper _mapper;
    private readonly PasswordEncrypter _passwordEncrypter;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly ITokenUserRepository _tokenUserRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ICpfValidator _cpfValidator;


    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository, 
        IUserWriteOnlyRepository writeOnlyRepository,
        PasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator,
        IUnityOfWork unityOfWork,
        IMapper mapper,
        ITokenUserRepository tokenUserRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        ICpfValidator cpfValidator)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _tokenUserRepository = tokenUserRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
        _cpfValidator = cpfValidator;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser req)
    {
        await Validate(req);

        var user = _mapper.Map<UserModel>(req);

        user.Password = _passwordEncrypter.Encrypt(req.Password);

        user.UserIdentifier = Guid.NewGuid();

        await _writeOnlyRepository.Add(user);

        await _unityOfWork.Commit();

        var refreshToken = await CreateAndSaveRefreshToken(user);

        return new ResponseRegisteredUser
        {
            Name = req.Name,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, "user"), //adicinado string
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Models.UserModel user)
    {
        var refreshToken = _refreshTokenGenerator.Generate();

        await _tokenUserRepository.SaveNewRefreshToken(new RefreshTokenUserModel
        {
            Value = refreshToken,
            UserId = user.Id
        });

        await _unityOfWork.Commit();

        return refreshToken;
    }

    private async Task Validate(RequestRegisterUser req)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(req);

        var emailExists = await _readOnlyRepository.ExistsActiveUserWithEmail(req.Email);
        if (emailExists) 
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.EMAIL_JA_REGISTRADO));

        var cpfExists = await _readOnlyRepository.ExistsActiveUserWithCpf(req.Cpf);
        if (cpfExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.CPF_JA_REGISTRADO));

        /*var isCpfValid = _cpfValidator.IsValid(req.Cpf);
        if(isCpfValid.IsFalse())
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.CPF_INCORRETO));*/

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
