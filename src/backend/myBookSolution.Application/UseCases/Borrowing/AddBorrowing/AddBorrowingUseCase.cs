using AutoMapper;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Book;
using myBookSolution.Domain.Repositories.Borrowing;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Domain.Services.LoggedCurator;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;
using MyRecipeBook.Domain.Extensions;

namespace myBookSolution.Application.UseCases.Borrowing.AddBorrowing;

public class AddBorrowingUseCase : IAddBorrowingUseCase
{
    private readonly IBorrowingReadOnlyRepository _readOnlyRepository;
    private readonly IBorrowingWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IBookReadOnlyRepository _bookReadOnlyRepository;
    private readonly IBookWriteOnlyRepository _bookWriteOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILoggedCurator _loggedCurator;

    public AddBorrowingUseCase(IBorrowingReadOnlyRepository readOnlyRepository,
        IBorrowingWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IBookReadOnlyRepository bookReadOnlyRepository,
        IBookWriteOnlyRepository bookWriteOnlyRepository,
        IMapper mapper,
        IUnityOfWork unityOfWork,
        ILoggedCurator loggedCurator)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _bookReadOnlyRepository = bookReadOnlyRepository;
        _bookWriteOnlyRepository = bookWriteOnlyRepository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _loggedCurator = loggedCurator;
    }

    public async Task<ResponseAddedBorrowing> Execute(RequestAddBorrowing req)
    {
        await Validate(req);

        var userId = await _userReadOnlyRepository.GetIdByCpf(req.UserCpf);

        var bookId = await _bookReadOnlyRepository.FindBookByIsbn(req.BookIsbn);

        var loggedCurator = await _loggedCurator.CuratorLogged();

        var borrowing = _mapper.Map<BorrowingModel>(req);

        borrowing.CuratorId = loggedCurator.Id;

        borrowing.UserId = userId;

        borrowing.BookId = bookId;

        await _writeOnlyRepository.Add(borrowing);

        await _unityOfWork.Commit();

        return new ResponseAddedBorrowing
        {
            BorrowingDate = borrowing.BorrowingDate,
        };
    }

    public async Task Validate(RequestAddBorrowing req)
    {
        var validator = new AddBorrowingValidator();

        var result = validator.Validate(req);

        var user = await _userReadOnlyRepository.GetUserByCpf(req.UserCpf);
        if (user == null)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.CPF_USUARIO_NAO_EXISTE));
        else
        {
            var userHasAnyActiveBorrowing = await _readOnlyRepository.IsAnyBorrowActive(user.Id);
            if (userHasAnyActiveBorrowing)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.USUARIO_PENDENCIAS));
        }


        var book = await _bookReadOnlyRepository.GetBookByIsbn(req.BookIsbn);
        if (book == null)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.ISBN_LIVRO_NAO_EXISTE));
        else
        {
            if (book.Quantity < 1)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.LIVRO_SEM_ESTOQUE));
            else
            {
                var decrementResult = await _bookWriteOnlyRepository.DecrementStock(book.Id);
                if(decrementResult.IsFalse())
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesExceptions.ERRO_EMPRESTIMO));
            }

        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnAddBorrowing(errorMessages);
        }

    }
}
