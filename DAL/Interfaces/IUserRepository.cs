using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        public void UpdateUser(User user);
        public void DeleteUser(User user);
        public void CreateUser(User user);
        public User Login(string name, byte[] password);
        public User GetUser(int UserId);
        public IEnumerable<User> GetAll();
        public void Save();

    }
}
