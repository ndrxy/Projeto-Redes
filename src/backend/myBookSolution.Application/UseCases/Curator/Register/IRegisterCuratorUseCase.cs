using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Curator.Register;

public interface IRegisterCuratorUseCase
{
    public Task<ResponseRegisteredUser> Execute(RequestRegisterUser req);
}
