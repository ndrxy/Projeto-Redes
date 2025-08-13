using System.Net;

namespace myBookSolution.Exceptions.ExceptionsBase;

public abstract class SolutionExceptions : SystemException
{
    protected SolutionExceptions(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
