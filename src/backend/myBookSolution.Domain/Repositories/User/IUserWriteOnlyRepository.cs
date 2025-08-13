using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    public Task Add(UserModel user);

}
