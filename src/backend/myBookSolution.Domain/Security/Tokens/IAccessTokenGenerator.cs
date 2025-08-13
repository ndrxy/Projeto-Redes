namespace myBookSolution.API.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier, string role); //adicionado string role
}
