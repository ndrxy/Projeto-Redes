using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class UnauthorizedException : SolutionExceptions
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
