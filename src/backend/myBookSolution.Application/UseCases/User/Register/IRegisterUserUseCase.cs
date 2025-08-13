using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<ResponseRegisteredUser> Execute(RequestRegisterUser req);
}
