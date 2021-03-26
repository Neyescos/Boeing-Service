namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        public IPlaneModelRepository Planes { get; }
        public IUserRepository Users { get; }
        public IPlanePartRepository Parts { get; }
    }
}
