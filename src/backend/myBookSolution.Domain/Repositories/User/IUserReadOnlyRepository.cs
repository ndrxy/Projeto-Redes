using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistsActiveUserWithEmail(string email);

    public Task<UserModel> GetByEmailAndPassword(string email, string password);

    public Task<bool> ExistsActiveUserWithIdentifier(Guid userIdentifier);

    public Task<bool> ExistsActiveUserWithCpf(string cpf);

    public Task<long> GetIdByCpf(string cpf);

    public Task<UserModel?> GetUserByCpf(string cpf);

}

