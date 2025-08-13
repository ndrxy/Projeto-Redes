using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Book.AddBook;

public interface IAddBookUseCase
{
    public Task<ResponseAddedBook> Execute(RequestAddBook req);
}
