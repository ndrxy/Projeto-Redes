using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Book;

public interface IBookWriteOnlyRepository
{
    public Task Add(BookModel book);

    public Task<bool> DecrementStock(long id);

    public Task<bool> IncrementStock(long id);
}
