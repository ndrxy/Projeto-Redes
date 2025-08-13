using AutoMapper;
using myBookSolution.API.Security.Tokens;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Author;
using myBookSolution.Domain.Services.LoggedCurator;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Author.AddAuthor;

public class AddAuthorUseCase : IAddAuthorUseCase
{
    private readonly IAuthorReadOnlyRepository _readOnlyRepository;
    private readonly IAuthorWriteOnlyRepository _writeOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILoggedCurator _loggedCurator;

    public AddAuthorUseCase(IAuthorReadOnlyRepository readOnlyRepository,
        IAuthorWriteOnlyRepository writeOnlyRepository,
        IMapper mapper,
        IUnityOfWork unityOfWork,
        ILoggedCurator loggedCurator)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _loggedCurator = loggedCurator;
    }

    public async Task<ResponseAddedAuthor> Execute(RequestAddAuthor req)
    {
        await Validate(req);

        var loggedCurator = await _loggedCurator.CuratorLogged();

        var author = _mapper.Map<AuthorModel>(req);

        author.CuratorId = loggedCurator.Id;

        await _writeOnlyRepository.Add(author);

        await _unityOfWork.Commit();

        return new ResponseAddedAuthor
        {
            Name = req.Name,
        };
    }

    private async Task Validate(RequestAddAuthor req)
    {
        var validator = new AddAuthorValidator();

        var result = validator.Validate(req);

        req.Name = req.Name.ToUpper();

        var authorExists = await _readOnlyRepository.AuthorAlreadyExists(req.Name);
        if (authorExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.AUTOR_JA_CADASTRADO));
    
        if(result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnAddAuthor(errorMessages);
        }

    }

}
