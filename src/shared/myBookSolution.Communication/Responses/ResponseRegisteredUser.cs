namespace myBookSolution.Communication.Responses;

public class ResponseRegisteredUser
{
    public string Name { get; set; } = string.Empty;
    public ResponseToken Tokens { get; set; } = default!;
}
