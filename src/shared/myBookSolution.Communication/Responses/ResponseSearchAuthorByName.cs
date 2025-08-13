namespace myBookSolution.Communication.Responses;

public class ResponseSearchAuthorByName
{
    public IList<string> AuthorsList { get; set; } = default!;
}
