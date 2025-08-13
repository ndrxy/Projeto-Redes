using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class RefreshTokenNotFoundException : SolutionExceptions
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesExceptions.SESSAO_EXPIRADA)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
