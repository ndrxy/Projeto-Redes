namespace myBookSolution.Communication.Responses;

public class ResponseSearchBookByName
{
    public IList<string> BooksList { get; set; } = default!;
}
