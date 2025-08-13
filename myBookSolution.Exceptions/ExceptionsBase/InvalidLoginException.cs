using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class InvalidLoginException : SolutionExceptions
{
    public InvalidLoginException() : base(ResourceMessagesExceptions.EMAIL_OU_SENHA_INCORRETOS)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
