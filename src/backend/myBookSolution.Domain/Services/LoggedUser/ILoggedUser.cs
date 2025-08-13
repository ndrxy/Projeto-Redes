using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<UserModel> UserLogged();
}
