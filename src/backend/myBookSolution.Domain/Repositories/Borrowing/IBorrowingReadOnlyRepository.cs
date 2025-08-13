using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Borrowing;

public interface IBorrowingReadOnlyRepository
{
    public Task<bool> IsAnyBorrowActive(long id);

    public Task<BorrowingModel?> GetBorrowingByUserId(long id);

    public Task<IList<BorrowingModel>> GetUserBorrowingHistoryById(long userId);
}
