using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Author.SearchAuthorByName;

public interface ISearchAuthorByNameUseCase
{
    public Task<ResponseSearchAuthorByName?> Execute(RequestSearchName req);
}
