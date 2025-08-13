using Microsoft.AspNetCore.Mvc;
using myBookSolution.Application.UseCases.Token.RefreshToken;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers;

public class UserRefreshTokenController : BookSolutionBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseToken), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUserRefreshTokenUseCase useCase,
        [FromBody] RequestNewToken req)
    {
        var response = await useCase.Execute(req);

        return Ok(response);
    }
}
