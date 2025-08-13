using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myBookSolution.API.Attributes;
using myBookSolution.Application.UseCases.Author.SearchAuthorByName;
using myBookSolution.Application.UseCases.Book.AddBook;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers;

public class BookController : BookSolutionBaseController
{
    [HttpPost]
    [AuthenticatedCurator]
    [Authorize(Roles = "curator")]
    [ProducesResponseType(typeof(ResponseAddedBook), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddBook([FromServices] IAddBookUseCase useCase,
        [FromBody] RequestAddBook req)
    {
        var result = await useCase.Execute(req);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [Authorize(Roles = "curator,user")]
    [ProducesResponseType(typeof(ResponseSearchBookByName), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuthorByName([FromServices] ISearchAuthorByNameUseCase useCase,
        [FromQuery] RequestSearchName req)
    {
        var result = await useCase.Execute(req);

        return Ok(result);
    }
}
