using Application.DTOs.Auth;
using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public AuthResponseDto Register(RegisterDto dto)
        {
            var existingUser = _userRepository.GetByEmail(dto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email already registered");
            }

            var hashedPassword = _passwordHasher.Hash(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Age = dto.Age,
                MonthlyIncome = dto.MonthlyIncome,
                IsBlocked = false,
                Role = "User"
            };

            _userRepository.Add(user);
            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Age = user.Age,
                    MonthlyIncome = user.MonthlyIncome,
                    IsBlocked = user.IsBlocked
                }
            };
        }
        public AuthResponseDto Login(LoginDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            bool isPasswordValid = _passwordHasher.Verify(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Age = user.Age,
                    MonthlyIncome = user.MonthlyIncome,
                    IsBlocked = user.IsBlocked
                }
            };
        }
    }
}

