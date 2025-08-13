using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.Book;
using myBookSolution.Infrastructure.Data;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.Book;

public class BookRepository : IBookReadOnlyRepository, IBookWriteOnlyRepository
{
    private readonly MyDbContext _dbContext;

    public BookRepository(MyDbContext context) => _dbContext = context;

    public async Task Add(BookModel book) => await _dbContext.AddAsync(book);

    public async Task<bool> BookAlreadyExists(string name) => await _dbContext.Books.AnyAsync(book => book.Title == name);

    public async Task<bool> IsbnAlreadyExists(string isbn) => await _dbContext.Books.AnyAsync(book => book.Isbn == isbn);

    public async Task<long> FindBookByIsbn(string isbn)
    {
        return await _dbContext.Books
        .Where(b => b.Isbn == isbn)
        .Select(b => b.Id)
        .FirstOrDefaultAsync();
    }

    public async Task<BookModel?> GetBookByIsbn(string isbn)
    {
        return await _dbContext
            .Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Isbn.Equals(isbn));
    }

    public async Task<bool> DecrementStock(long id)
    {
        var rowsAffected = await _dbContext.Books
        .Where(b => b.Id == id && b.Quantity > 0)
        .ExecuteUpdateAsync(s =>
            s.SetProperty(b => b.Quantity, b => b.Quantity - 1));

        return rowsAffected > 0;
    }

    public async Task<bool> IncrementStock(long id)
    {
        var rowsAffected = await _dbContext.Books
        .Where(b => b.Id == id && b.Quantity > 0)
        .ExecuteUpdateAsync(s =>
            s.SetProperty(b => b.Quantity, b => b.Quantity + 1));

        return rowsAffected > 0;
    }

    public async Task<IList<string>?> SearchByTitle(string title)
    {
        title = title.Trim().ToUpper();

        return await _dbContext.Books.Select(a => a.Title).Take(5).ToListAsync();
    }
}
