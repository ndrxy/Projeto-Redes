using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Book;

public interface IBookReadOnlyRepository
{
    public Task<bool> BookAlreadyExists(string name);

    public Task<bool> IsbnAlreadyExists(string isbn);

    public Task<long> FindBookByIsbn(string isbn);

    public Task<BookModel?> GetBookByIsbn(string isbn);

    public Task<IList<string>?> SearchByTitle(string title);
}
