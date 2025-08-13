using AutoMapper;
using myBookSolution.Communication.Requests;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories.Author;
using myBookSolution.Exceptions.ExceptionsBase;

namespace myBookSolution.Application.UseCases.Author.SearchAuthorByName;

public class SearchAuthorByNameUseCase : ISearchAuthorByNameUseCase
{
    private readonly IAuthorReadOnlyRepository _readOnlyRepository;

    public SearchAuthorByNameUseCase(IAuthorReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<ResponseSearchAuthorByName?> Execute(RequestSearchName req)
    {
        await Validate(req);

        var authorsList = await _readOnlyRepository.SearchByName(req.Name);

        return new ResponseSearchAuthorByName
        {
            AuthorsList = authorsList
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

