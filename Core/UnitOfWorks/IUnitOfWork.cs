namespace  Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync(); // SaveChangesAsync'in karşılığı
        void Commit();      // SaveChanges'in karşılığı
    }
}