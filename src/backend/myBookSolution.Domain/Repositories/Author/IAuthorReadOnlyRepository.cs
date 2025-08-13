using myBookSolution.Communication.Responses;

namespace myBookSolution.Domain.Repositories.Author;

public interface IAuthorReadOnlyRepository
{
    public Task<bool> AuthorAlreadyExists(string name);

    public Task<long> GetAuthorIdByName(string name);

    public Task<IList<string>?> SearchByName(string name);
}
