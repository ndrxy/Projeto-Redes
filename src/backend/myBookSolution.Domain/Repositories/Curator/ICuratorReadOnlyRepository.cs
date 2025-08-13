using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Curator;

public interface ICuratorReadOnlyRepository
{
    public Task<bool> ExistsActiveCuratorWithEmail(string email);

    public Task<bool> ExistsActiveCuratorWithCpf(string cpf);

    public Task<CuratorModel> GetByEmailAndPassword(string email, string password);

    public Task<bool> ExistsActiveCuratorWithIdentifier(Guid userIdentifier);
}
