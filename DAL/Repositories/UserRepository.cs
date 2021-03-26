using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BoengServiceWebSiteContext _context;
        public UserRepository(BoengServiceWebSiteContext context)
        {
            _context = context;
        }
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            Save();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            Save();
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.Include(u => u.PlaneModels).AsNoTracking().ToList();
            return users;
        }

        public User GetUser(int UserId)
        {
            return _context.Users.Find(UserId);
        }

        public User Login(string name, byte[] password)
        {
            var user = _context.Users?.Where(u => u.Name == name && u.Password == password).FirstOrDefault();
            return user;
        }


        public void UpdateUser(User user)
        {
            var dbUser = GetAll().FirstOrDefault(u => u.Id == user.Id);
            _context.ChangeTracker.Clear();
            _context.Update(dbUser);
            dbUser.Name = user.Name;
            if (user.Password.Length > 0)
            {
                dbUser.Password = user.Password;
            }

            if (user.PlaneModels.Count > dbUser.PlaneModels.Count)
            {
                AddPlanes(user.PlaneModels, dbUser.PlaneModels, dbUser);
            }
            else if (user.PlaneModels.Count < dbUser.PlaneModels.Count)
            {
                RemovePlanes(user.PlaneModels, dbUser.PlaneModels, dbUser);
            }
            Save();
        }

        private void RemovePlanes(ICollection<PlaneModel> userPlanes, ICollection<PlaneModel> dbUserPlanes, User dbUser)
        {
            var removedPlanes = dbUserPlanes.Where(dbUserPlane => !userPlanes.Any(userPlane => dbUserPlane.Id == userPlane.Id)).ToList();
            foreach (var plane in dbUserPlanes)
            {
                _context.Entry(plane).State = EntityState.Detached;
            }
            var users = new List<User>();
            foreach (var plane in removedPlanes)
            {
                _context.Update(plane);
                users = plane.Users.ToList();
                plane.Users = null;
                Save();
                users.Remove(dbUser);
                _context.Update(plane);
                plane.Users = users;
                Save();
            }
        }

        private void AddPlanes(ICollection<PlaneModel> userPlanes, ICollection<PlaneModel> dbUserPlanes, User dbUser)
        {
            var addedPlanes = userPlanes.Where(userPlane => !dbUserPlanes.Any(dbUserPlane => userPlane.Id == dbUserPlane.Id));
            foreach (var plane in dbUser.PlaneModels)
            {
                _context.Entry(plane).State = EntityState.Detached;
            }
            var users = new List<User>();
            var planes = new List<PlaneModel>();
            foreach (var plane in addedPlanes)
            {
               _context.ChangeTracker.Clear();

                _context.Update(plane);
                _context.Update(dbUser);
                users = plane.Users.ToList();
                planes = dbUser.PlaneModels.ToList();
                dbUser.PlaneModels = null;
                plane.Users = null;
                Save();
                planes.Add(plane);
                dbUser.PlaneModels = planes;
                users.Add(dbUser);
                plane.Users = users;
                _context.Update(plane);
                Save();
            }
           
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
