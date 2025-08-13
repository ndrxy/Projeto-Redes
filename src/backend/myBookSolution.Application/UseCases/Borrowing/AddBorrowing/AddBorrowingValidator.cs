using FluentValidation;
using myBookSolution.Communication.Requests;
using myBookSolution.Exceptions;

namespace myBookSolution.Application.UseCases.Borrowing.AddBorrowing;

public class AddBorrowingValidator : AbstractValidator<RequestAddBorrowing>
{
    public AddBorrowingValidator()
    {
        RuleFor(borrowing => borrowing.UserCpf).NotEmpty().WithMessage(ResourceMessagesExceptions.USUARIO_NAO_INFORMADO);
        RuleFor(borrowing => borrowing.BookIsbn).NotEmpty().WithMessage(ResourceMessagesExceptions.LIVRO_NAO_INFORMADO);
    }
}
