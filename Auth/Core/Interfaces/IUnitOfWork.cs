namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

        void Commit();
    }
}