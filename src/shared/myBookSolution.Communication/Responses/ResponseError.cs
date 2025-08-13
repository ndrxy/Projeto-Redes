namespace myBookSolution.Communication.Responses;

public class ResponseError
{
    public IList<string> Errors { get; set; }

    public ResponseError(IList<string> errors) => Errors = errors;

    public bool TokenIsExpired { get; set; }

    public ResponseError(string error)
    {
        Errors = new List<string>
        {
            error
        };
    }
}
