using myBookSolution.Application.UseCases.Author.SearchAuthorByName;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories.Book;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Book.SearchBookByName;

public class SearchBookByNameUseCase : ISearchBookByNameUseCase
{
    private readonly IBookReadOnlyRepository _readOnlyRepository;

    public SearchBookByNameUseCase(IBookReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<ResponseSearchBookByName?> Execute(RequestSearchName req)
    {
        await Validate(req);

        var booksList = await _readOnlyRepository.SearchByTitle(req.Name);

        return new ResponseSearchBookByName
        {
            BooksList = booksList
        };
    }

    public async Task Validate(RequestSearchName req)
    {
        var validator = new SearchAuthorValidator();

        var result = validator.Validate(req);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new SearchError();
        }
    }
}
