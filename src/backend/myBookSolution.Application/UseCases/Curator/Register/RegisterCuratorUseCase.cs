using AutoMapper;
using myBookSolution.API.Security.Tokens;
using myBookSolution.Application.Services.Cryptography;
using myBookSolution.Application.UseCases.User.Register;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Curator;
using myBookSolution.Domain.Repositories.Token;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Curator.Register;

public class RegisterCuratorUseCase : IRegisterCuratorUseCase
{
    private readonly ICuratorReadOnlyRepository _readOnlyRepository;
    private readonly ICuratorWriteOnlyRepository _writeOnlyRepository;
    private readonly IMapper _mapper;
    private readonly PasswordEncrypter _passwordEncrypter;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ITokenCuratorRepository _tokenCuratorRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterCuratorUseCase(
        ICuratorReadOnlyRepository readOnlyRepository,
        ICuratorWriteOnlyRepository writeOnlyRepository,
        PasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenCuratorRepository tokenCuratorRepository,
        IUnityOfWork unityOfWork,
        IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenCuratorRepository = tokenCuratorRepository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser req)
    {
        Validate(req);

        var curator = _mapper.Map<CuratorModel>(req);

        curator.Password = _passwordEncrypter.Encrypt(req.Password);

        curator.CuratorIdentifier = Guid.NewGuid();

        await _writeOnlyRepository.Add(curator);

        await _unityOfWork.Commit();

        var refreshToken = await CreateAndSaveRefreshToken(curator);

        return new ResponseRegisteredUser
        {
            Name = req.Name,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(curator.CuratorIdentifier, "curator"), //adicinado string
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Models.CuratorModel curator)
    {
        var refreshToken = _refreshTokenGenerator.Generate();

        await _tokenCuratorRepository.SaveNewRefreshToken(new RefreshTokenCuratorModel
        {
            Value = refreshToken,
            CuratorId = curator.Id
        });

        await _unityOfWork.Commit();

        return refreshToken;
    }

    private async void Validate(RequestRegisterUser req)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(req);

        var emailExists = await _readOnlyRepository.ExistsActiveCuratorWithEmail(req.Email);
        if (emailExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.EMAIL_JA_REGISTRADO));

        var cpfExists = await _readOnlyRepository.ExistsActiveCuratorWithCpf(req.Cpf);
        if (cpfExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.CPF_JA_REGISTRADO));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
