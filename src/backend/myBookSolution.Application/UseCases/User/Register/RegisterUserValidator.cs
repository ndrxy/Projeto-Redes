using FluentValidation;
using myBookSolution.Communication.Requests;
using myBookSolution.Exceptions;

namespace myBookSolution.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesExceptions.NOME_VAZIO);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesExceptions.EMAIL_VAZIO);
        RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesExceptions.EMAIL_INCORRETO);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessagesExceptions.SENHA_VAZIA);
        RuleFor(user => user.Cpf.Length).Equal(11).WithMessage(ResourceMessagesExceptions.CPF_INCORRETO);
    }
}
