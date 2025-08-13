using myBookSolution.Domain.Models;

namespace myBookSolution.Domain.Repositories.Curator;

public interface ICuratorWriteOnlyRepository
{
    public Task Add(CuratorModel curator);
}
