using FluentValidation;
using myBookSolution.Communication.Requests;
using myBookSolution.Exceptions;

namespace myBookSolution.Application.UseCases.Author.SearchAuthorByName;

public class SearchAuthorValidator : AbstractValidator<RequestSearchName>
{
    public SearchAuthorValidator()
    {
        RuleFor(author => author.Name).NotEmpty().WithMessage(ResourceMessagesExceptions.NOME_VAZIO);
    }
}
