using Application;
using Application.DTOs.User;
using Application.Services;
using Infrastructure;
using Domain;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public void GetUserById_WithValidId_ReturnsUserDto()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Age = 25,
                MonthlyIncome = 5000,
                IsBlocked = false
            };

            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(user);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public void GetUserById_WithInvalidId_ThrowsException()
        {
            // Arrange
            var userId = 999;
            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns((User)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _userService.GetUserById(userId));
        }

        [Fact]
        public void BlockUser_WithValidId_BlocksUser()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                IsBlocked = false
            };

            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(user);

            // Act
            var result = _userService.BlockUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsBlocked);
            _userRepositoryMock.Verify(x => x.Update(It.Is<User>(u => u.IsBlocked == true)), Times.Once);
        }

        [Fact]
        public void UnblockUser_WithValidId_UnblocksUser()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                IsBlocked = true
            };

            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(user);

            // Act
            var result = _userService.UnblockUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsBlocked);
            _userRepositoryMock.Verify(x => x.Update(It.Is<User>(u => u.IsBlocked == false)), Times.Once);
        }
    }
}


