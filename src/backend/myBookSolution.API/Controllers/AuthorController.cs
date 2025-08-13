using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myBookSolution.API.Attributes;
using myBookSolution.Application.UseCases.Author.AddAuthor;
using myBookSolution.Application.UseCases.Author.SearchAuthorByName;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers;

public class AuthorController : BookSolutionBaseController
{
    [HttpPost]
    [AuthenticatedCurator]
    [Authorize(Roles = "curator")]
    [ProducesResponseType(typeof(ResponseAddedAuthor), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddAuthor([FromServices] IAddAuthorUseCase useCase, 
        [FromBody] RequestAddAuthor req)
    {
        var result = await useCase.Execute(req);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [Authorize(Roles = "curator,user")]
    [ProducesResponseType(typeof(ResponseSearchAuthorByName), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuthorByName([FromServices] ISearchAuthorByNameUseCase useCase,
        [FromQuery] RequestSearchName req)
    {
        var result = await useCase.Execute(req);

        return Ok(result);
    }
}
