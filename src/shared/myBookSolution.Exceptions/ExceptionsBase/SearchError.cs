using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class SearchError : SolutionExceptions
{
    public SearchError() : base(ResourceMessagesExceptions.CAMPO_BUSCA_VAZIO)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
