using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        public UserDto Login(string login, string password);
        public void Registration(UserDto user);
        public void DeleteUser(UserDto user);
        public void UpdateUser(UserDto user);
        public UserDto GetUser(int UserId);
        public IEnumerable<UserDto> GetUsers();

    }
}
