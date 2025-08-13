using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.Application.UseCases.Token.RefreshToken;

public interface IUserRefreshTokenUseCase
{
    Task<ResponseToken> Execute(RequestNewToken request);
}
