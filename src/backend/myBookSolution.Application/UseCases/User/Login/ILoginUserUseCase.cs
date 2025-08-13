using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.User.Login;

public interface ILoginUserUseCase
{
    public Task<ResponseRegisteredUser> Execute(RequestUserLogin req);
}
