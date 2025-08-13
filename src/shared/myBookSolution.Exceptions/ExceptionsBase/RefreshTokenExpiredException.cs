using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException : SolutionExceptions
{
    public RefreshTokenExpiredException() : base(ResourceMessagesExceptions.TOKEN_EXPIRADO)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
