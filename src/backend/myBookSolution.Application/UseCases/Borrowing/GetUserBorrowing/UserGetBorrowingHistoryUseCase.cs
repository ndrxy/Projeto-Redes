using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories.Borrowing;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Domain.Services.LoggedUser;

namespace myBookSolution.Application.UseCases.Borrowing.GetUserBorrowing;

public class UserGetBorrowingHistoryUseCase : IUserGetBorrowingHistoryUseCase
{
    private readonly IBorrowingReadOnlyRepository _borrowingReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public UserGetBorrowingHistoryUseCase(IUserReadOnlyRepository readOnlyRepository,
        IBorrowingReadOnlyRepository borrowingReadOnlyRepository,
        ILoggedUser loggedUser)
    {
        _borrowingReadOnlyRepository = borrowingReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<IList<ResponseBorrowingHistory>?> Execute()
    {
        var user = await _loggedUser.UserLogged();

        var borrowings = await _borrowingReadOnlyRepository.GetUserBorrowingHistoryById(user.Id);

        return borrowings.Select(b => new ResponseBorrowingHistory
        {
            BookName = b.Book.Title,
            BorrowingDate = b.BorrowingDate,
        }).ToList();

    }
}
