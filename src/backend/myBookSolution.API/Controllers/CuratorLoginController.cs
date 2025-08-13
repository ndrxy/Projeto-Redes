using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myBookSolution.Application.UseCases.Curator.Login;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers
{
    public class CuratorLoginController : BookSolutionBaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] ILoginCuratorUseCase useCase,
            [FromBody] RequestUserLogin req)
        {
            var response = await useCase.Execute(req);

            return Ok(response);
        }
    }
}
