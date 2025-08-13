using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class UserNotFoundException : SolutionExceptions
{
    public UserNotFoundException() : base(ResourceMessagesExceptions.CPF_USUARIO_NAO_EXISTE)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
