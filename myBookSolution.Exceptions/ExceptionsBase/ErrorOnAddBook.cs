using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public class ErrorOnAddBook : SolutionExceptions
{
    public IList<string> _errorMessages { get; set; }

    public ErrorOnAddBook(IList<string> errorMessages) : base(string.Empty)
    {
        _errorMessages = errorMessages;
    }

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
