using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Token.RefreshToken;

public interface ICuratorRefreshTokenUseCase
{
    Task<ResponseToken> Execute(RequestNewToken request);
}
