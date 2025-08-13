using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Curator.Profile;

public interface IGetCuratorProfileUseCase
{
    public Task<ResponseUserProfile> Execute();
}
