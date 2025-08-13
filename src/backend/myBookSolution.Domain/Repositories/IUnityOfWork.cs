namespace myBookSolution.Domain.Repositories;

public interface IUnityOfWork
{
    public Task Commit();
}
