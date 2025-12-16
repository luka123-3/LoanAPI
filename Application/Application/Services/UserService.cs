using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Enums;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Age = user.Age,
                MonthlyIncome = user.MonthlyIncome,
                IsBlocked = user.IsBlocked
            };
        }

        public UserDto BlockUser(int userId, DateTime? blockedUntil = null)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.IsBlocked = true;
            user.BlockedUntil = blockedUntil;
            _userRepository.Update(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Age = user.Age,
                MonthlyIncome = user.MonthlyIncome,
                IsBlocked = user.IsBlocked
            };
        }

        public UserDto UnblockUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.IsBlocked = false;
            user.BlockedUntil = null;
            _userRepository.Update(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Age = user.Age,
                MonthlyIncome = user.MonthlyIncome,
                IsBlocked = user.IsBlocked
            };
        }

        public bool IsUserBlocked(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (user.IsBlocked && user.BlockedUntil.HasValue)
            {
                if (user.BlockedUntil.Value < DateTime.UtcNow)
                {
                    user.IsBlocked = false;
                    user.BlockedUntil = null;
                    _userRepository.Update(user);
                    return false;
                }
            }

            return user.IsBlocked;
        }
    }
}


