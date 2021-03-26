using DAL.Interfaces;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPlaneModelRepository Planes { get; }
        public IUserRepository Users { get; }
        public IPlanePartRepository Parts { get; }
        public UnitOfWork(IPlaneModelRepository planes, IUserRepository users, IPlanePartRepository parts)
        {
            Planes = planes;
            Users = users;
            Parts = parts;
        }
    }
}
