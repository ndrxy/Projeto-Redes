using Microsoft.AspNetCore.Mvc;
using myBookSolution.Application.UseCases.User.Login;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers
{
    public class LoginController : BookSolutionBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] ILoginUserUseCase useCase,
            [FromBody] RequestUserLogin req)
        {
            var response = await useCase.Execute(req);

            return Ok(response);
        }
    }
}
