using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myBookSolution.API.Attributes;
using myBookSolution.Application.UseCases.Curator.Profile;
using myBookSolution.Application.UseCases.Curator.Register;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers;

[Authorize(Roles = "curator")] //adicionado
public class CuratorController : BookSolutionBaseController
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterCuratorUseCase useCase,
        [FromBody] RequestRegisterUser req)
    {

        var result = await useCase.Execute(req);

        return Created(string.Empty, result);
    }

    [AuthenticatedCurator]
    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfile), StatusCodes.Status200OK)]
    //[Authorize(Roles = "curator")]
    public async Task<IActionResult> GetCuratorProfile([FromServices] IGetCuratorProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }
}
