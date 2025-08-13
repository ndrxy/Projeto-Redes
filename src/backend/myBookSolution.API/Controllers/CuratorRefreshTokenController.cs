using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myBookSolution.API.Attributes;
using myBookSolution.Application.UseCases.Token.RefreshToken;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers;

public class CuratorRefreshTokenController : BookSolutionBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseToken), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] ICuratorRefreshTokenUseCase useCase,
        [FromBody] RequestNewToken req)
    {
        var response = await useCase.Execute(req);

        return Ok(response);
    }
}
