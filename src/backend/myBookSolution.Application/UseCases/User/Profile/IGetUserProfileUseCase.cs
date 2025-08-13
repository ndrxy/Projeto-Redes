using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfile> Execute();
}
