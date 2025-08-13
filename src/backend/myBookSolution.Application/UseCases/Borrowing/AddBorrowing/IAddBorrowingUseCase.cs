using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Borrowing.AddBorrowing;

public interface IAddBorrowingUseCase
{
    public Task<ResponseAddedBorrowing> Execute(RequestAddBorrowing req);
}
