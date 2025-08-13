using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myBookSolution.API.Attributes;
using myBookSolution.Application.UseCases.Author.SearchAuthorByName;
using myBookSolution.Application.UseCases.Borrowing.AddBorrowing;
using myBookSolution.Application.UseCases.Borrowing.Devolution;
using myBookSolution.Application.UseCases.Borrowing.GetUserBorrowing;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;

namespace myBookSolution.API.Controllers;

public class BorrowingController : BookSolutionBaseController
{
    [HttpPost]
    [AuthenticatedCurator]
    [Authorize(Roles = "curator")]
    [ProducesResponseType(typeof(ResponseAddedBorrowing), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddBorrowing([FromServices] IAddBorrowingUseCase useCase,
        [FromBody] RequestAddBorrowing req)
    {
        var result = await useCase.Execute(req);

        return Created(string.Empty, result);
    }

    [HttpPut]
    [AuthenticatedCurator]
    [Authorize(Roles = "curator")]
    [ProducesResponseType(typeof(ResponseAddedBorrowing), StatusCodes.Status201Created)]
    public async Task<IActionResult> Devolution([FromServices] IDevolutionBookUseCase useCase,
        [FromBody] RequestUserDevolution req)
    {
        var result = await useCase.Execute(req);

        return Ok(result);
    }

    [HttpGet]
    [AuthenticatedUser]
    [Authorize(Roles = "user")]
    [ProducesResponseType(typeof(ResponseBorrowingHistory), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAuthorByName([FromServices] IUserGetBorrowingHistoryUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }

}
