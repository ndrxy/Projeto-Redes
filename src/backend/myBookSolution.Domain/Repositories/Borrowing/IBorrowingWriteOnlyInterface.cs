using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Borrowing;

public interface IBorrowingWriteOnlyRepository
{
    public Task Add(BorrowingModel book);

    public Task<bool> ReturnBook(long id);
}
