
using AutoMapper;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories;
using myBookSolution.Domain.Repositories.Book;
using myBookSolution.Domain.Repositories.Borrowing;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Borrowing.Devolution;

public class DevolutionUseCase : IDevolutionBookUseCase
{
    private readonly IBorrowingReadOnlyRepository _readOnlyRepository;
    private readonly IBorrowingWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IBookWriteOnlyRepository _bookWriteOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;

    public DevolutionUseCase(IBorrowingReadOnlyRepository readOnlyRepository,
        IBorrowingWriteOnlyRepository writeOnlyRepository,
        IBookWriteOnlyRepository bookWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IMapper mapper,
        IUnityOfWork unityOfWork)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _bookWriteOnlyRepository = bookWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
    }

    public async Task<ResponseAddedBorrowing> Execute(RequestUserDevolution req)
    {
        UserModel? user = await _userReadOnlyRepository.GetUserByCpf(req.Cpf);
        if (user == null)
            throw new UserNotFoundException();

        var borrowing = await _readOnlyRepository.GetBorrowingByUserId(user.Id);
        if (borrowing == null)
            throw new Exception(ResourceMessagesExceptions.USUARIO_SEM_EMPRESTIMO);

        var changeBorrowStatus = await _writeOnlyRepository.ReturnBook(borrowing.Id);

        if (!changeBorrowStatus)
            throw new Exception(ResourceMessagesExceptions.ERRO_EMPRESTIMO);

        var bookIncrement = await _bookWriteOnlyRepository.IncrementStock(borrowing.BookId);
        if(!bookIncrement)
            throw new Exception();

        await _unityOfWork.Commit();

        return new ResponseAddedBorrowing
        {
            BorrowingDate = DateTime.Now,
        };
    }
}
