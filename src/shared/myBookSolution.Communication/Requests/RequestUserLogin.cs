using System.Globalization;

namespace myBookSolution.Communication.Requests;

public class RequestUserLogin
{
    public string Email {  get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
