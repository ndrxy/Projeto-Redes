using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Borrowing.GetUserBorrowing;

public interface IUserGetBorrowingHistoryUseCase
{
    public Task<IList<ResponseBorrowingHistory>?> Execute();
}
