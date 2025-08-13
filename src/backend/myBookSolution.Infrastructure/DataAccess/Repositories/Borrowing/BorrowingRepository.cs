using Microsoft.EntityFrameworkCore;
using myBookSolution.Domain.Models;
using myBookSolution.Domain.Repositories.Borrowing;
using myBookSolution.Infrastructure.Data;
using System.Linq;

namespace myBookSolution.Infrastructure.DataAccess.Repositories.Borrowing;

public class BorrowingRepository : IBorrowingReadOnlyRepository, IBorrowingWriteOnlyRepository
{
    private readonly MyDbContext _dbContext;
    public BorrowingRepository(MyDbContext context)
    {
        _dbContext = context;
    }

    public async Task Add(BorrowingModel book) => await _dbContext.Borrowings.AddAsync(book);

    public async Task<bool> IsAnyBorrowActive(long id)
    {
        return await _dbContext.Borrowings
        .AnyAsync(b => b.UserId == id
                    && b.Status == false);
    }

    public async Task<BorrowingModel?> GetBorrowingByUserId(long id)
    {
        return await _dbContext.Borrowings
        .FirstOrDefaultAsync(b => b.UserId == id);
    }

    public async Task<bool> ReturnBook(long id)
    {
        int rowsAffected = await _dbContext.Borrowings.Where(b => b.UserId == id && b.Status == false).ExecuteUpdateAsync(s => s.SetProperty(s => s.Status, s => true));
    
        return rowsAffected > 0;
    }

    public async Task<IList<BorrowingModel>> GetUserBorrowingHistoryById(long userId)
    {
        return await _dbContext.Borrowings
        .Where(b => b.UserId == userId)
        .Include(b => b.Book) // para trazer o título junto
        .OrderByDescending(b => b.BorrowingDate)
        .ToListAsync();
    }
}
