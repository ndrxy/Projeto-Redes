namespace myBookSolution.Communication.Responses;

public class ResponseBorrowingHistory
{
    public string BookName { get; set; } = string.Empty;
    public DateTime BorrowingDate { get; set; }
}
