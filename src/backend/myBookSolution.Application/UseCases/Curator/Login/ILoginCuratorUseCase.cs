using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Curator.Login;

public interface ILoginCuratorUseCase
{
    public Task<ResponseRegisteredUser> Execute(RequestUserLogin req);
}
