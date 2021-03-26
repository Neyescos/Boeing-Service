using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unit;
        private readonly Mapper _mapper;
        private readonly IEncodingService _encodingService;

        public UserService(IUnitOfWork uow, IMapperProfile mapperProfile, IEncodingService encoding)
        {
            _encodingService = encoding;
            _mapper = mapperProfile.GetMapper();
            _unit = uow;
        }

        public void DeleteUser(UserDto user)
        {
            try
            {
                var userEntity = new User
                {
                    Id = user.Id,
                    Password = _encodingService.CalculateSHA256(user.Password),
                    Name = user.Name,
                    PlaneModels = _mapper.Map<ICollection<PlaneModel>>(user.PlaneModels),
                    RegistrationDate = user.RegistrationDate,
                    Role = user.Role
                };
                _unit.Users.DeleteUser(userEntity);
            }
            catch
            {
                //catch just not to stop the program
            }
        }

        public UserDto GetUser(int UserId)
        {
            try
            {
                return _mapper.Map<UserDto>(_unit.Users.GetUser(UserId));
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<UserDto> GetUsers()
        {
            try
            {
                return _mapper.Map<IEnumerable<UserDto>>(_unit.Users.GetAll());
            }
            catch
            {
                return new List<UserDto>();
            }
        }

        public UserDto Login(string login, string password)
        {
            try
            {
                var user = _unit.Users.Login(login, _encodingService.CalculateSHA256(password));
                var UserDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    PlaneModels = _mapper.Map<ICollection<PlaneModelDto>>(user.PlaneModels),
                    RegistrationDate = user.RegistrationDate,
                    Role = user.Role,
                    Password = string.Empty
                };
                return UserDto;
            }
            catch
            {
                return null;
            }
        }

        public void Registration(UserDto user)
        {
            try
            {
                var userEntity = new User
                {
                    Password = _encodingService.CalculateSHA256(user.Password),
                    Name = user.Name,
                    PlaneModels = _mapper.Map<ICollection<PlaneModel>>(user.PlaneModels),
                    RegistrationDate = user.RegistrationDate,
                    Role = user.Role
                };
                _unit.Users.CreateUser(userEntity);
            }
            catch
            {
                //catch just not to stop the program
            }
        }

        public void UpdateUser(UserDto user)
        {
            try
            {
                _unit.Users.UpdateUser(_mapper.Map<User>(user));
            }
            catch
            {
                //catch just not to stop the program
            }


        }
    }
}
