using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Services.LoggedCurator;

public interface ILoggedCurator
{
    public Task<CuratorModel> CuratorLogged();
}
