using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Author;

public interface IAuthorWriteOnlyRepository
{
    public Task Add(AuthorModel author);
}
