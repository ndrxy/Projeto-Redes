namespace myBookSolution.Communication.Requests;

public class RequestAddBook
{
    public string Title { get; set; } = string.Empty;

    public string Description {  get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string Isbn { get; set; }
}
