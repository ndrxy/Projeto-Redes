using AutoMapper;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Author;
using myBookSolution.Domain.Repositories.Book;
using myBookSolution.Domain.Services.LoggedCurator;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;
using MyRecipeBook.Domain.Extensions;

namespace myBookSolution.Application.UseCases.Book.AddBook;

public class AddBookUseCase : IAddBookUseCase
{
    private readonly IBookReadOnlyRepository _readOnlyRepository;
    private readonly IBookWriteOnlyRepository _writeOnlyRepository;
    private readonly IAuthorReadOnlyRepository _authorReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILoggedCurator _loggedCurator;

    public AddBookUseCase(IBookReadOnlyRepository readOnlyRepository,
        IBookWriteOnlyRepository writeOnlyRepository,
        IMapper mapper,
        IUnityOfWork unityOfWork,
        ILoggedCurator loggedCurator,
        IAuthorReadOnlyRepository authorReadOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _loggedCurator = loggedCurator;
        _authorReadOnlyRepository = authorReadOnlyRepository;
    }

    public async Task<ResponseAddedBook> Execute(RequestAddBook req)
    {
        await Validate(req);

        var loggedCurator = await _loggedCurator.CuratorLogged();

        var book = _mapper.Map<BookModel>(req);

        book.CuratorId = loggedCurator.Id;

        var authorId = await _authorReadOnlyRepository.GetAuthorIdByName(req.AuthorName);

        book.AuthorId = authorId;

        await _writeOnlyRepository.Add(book);

        await _unityOfWork.Commit();

        return new ResponseAddedBook
        {
            Title = req.Title,
        };
    }

    public async Task Validate(RequestAddBook req)
    {
        var validator = new AddBookValidator();

        var result = validator.Validate(req);

        var isbnExists = await _readOnlyRepository.IsbnAlreadyExists(req.Isbn);
        if (isbnExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.ISBN_JA_CADASTRADO));

        var searchAuthor = req.AuthorName.ToUpper();

        var authorExists = await _authorReadOnlyRepository.AuthorAlreadyExists(searchAuthor);
        if(authorExists.IsFalse())
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.AUTOR_INFORMADO_NAO_EXISTE));

        req.Title = req.Title.ToUpper();

        var bookExists = await _readOnlyRepository.BookAlreadyExists(req.Title);
        if (bookExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.LIVRO_JA_CADASTRADO));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnAddBook(errorMessages);
        }
    }
}
