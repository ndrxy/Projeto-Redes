using FluentValidation;
using myBookSolution.Communication.Requests;
using myBookSolution.Exceptions;

namespace myBookSolution.Application.UseCases.Book.AddBook;

public class AddBookValidator : AbstractValidator<RequestAddBook>
{
    public AddBookValidator()
    {
        RuleFor(book => book.Title).NotEmpty().WithMessage(ResourceMessagesExceptions.TITULO_LIVRO_VAZIO);
        RuleFor(book => book.Description).NotEmpty().WithMessage(ResourceMessagesExceptions.DESCRICAO_LIVRO_VAZIA);
        RuleFor(book => book.Quantity).NotEmpty().GreaterThan(0).WithMessage(ResourceMessagesExceptions.ESTOQUE_INCORRETO);
        RuleFor(book => book.Isbn).NotEmpty().WithMessage(ResourceMessagesExceptions.ISBN_VAZIO);
    }
}
