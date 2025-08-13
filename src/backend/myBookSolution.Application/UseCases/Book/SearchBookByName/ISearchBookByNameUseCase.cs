using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Book.SearchBookByName;

public interface ISearchBookByNameUseCase
{
    public Task<ResponseSearchBookByName?> Execute(RequestSearchName req);
}
