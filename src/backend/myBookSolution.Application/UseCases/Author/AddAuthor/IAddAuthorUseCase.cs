using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Author.AddAuthor;

public interface IAddAuthorUseCase
{
    public Task<ResponseAddedAuthor> Execute(RequestAddAuthor req);
}
