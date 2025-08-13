using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Borrowing.Devolution;

public interface IDevolutionBookUseCase
{
    public Task<ResponseAddedBorrowing> Execute(RequestUserDevolution req);
}
