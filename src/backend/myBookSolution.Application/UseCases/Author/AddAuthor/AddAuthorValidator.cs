using FluentValidation;
using myBookSolution.Communication.Requests;
using myBookSolution.Exceptions;

namespace myBookSolution.Application.UseCases.Author.AddAuthor;

public class AddAuthorValidator : AbstractValidator<RequestAddAuthor>
{
    public AddAuthorValidator()
    {
        RuleFor(author => author.Name).NotEmpty().WithMessage(ResourceMessagesExceptions.NOME_AUTOR_VAZIO);
        RuleFor(author => author.Description).NotEmpty().WithMessage(ResourceMessagesExceptions.DESCRICAO_AUTOR_VAZIA);
    }
}
