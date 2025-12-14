using Application;
using Application.DTOs.Auth;
using Application.Services;
using Infrastructure;
using Domain;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenServiceMock = new Mock<ITokenService>();
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object);
        }

        [Fact]
        public void Register_WithValidData_ReturnsAuthResponse()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Age = 25,
                MonthlyIncome = 5000
            };

            _userRepositoryMock.Setup(x => x.GetByEmail(registerDto.Email)).Returns((User)null);
            _passwordHasherMock.Setup(x => x.Hash(registerDto.Password)).Returns("hashed_password");
            _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("test_token");

            // Act
            var result = _authService.Register(registerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test_token", result.Token);
            Assert.NotNull(result.User);
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void Register_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "existing@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Age = 25,
                MonthlyIncome = 5000
            };

            var existingUser = new User { Email = registerDto.Email };
            _userRepositoryMock.Setup(x => x.GetByEmail(registerDto.Email)).Returns(existingUser);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _authService.Register(registerDto));
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new User
            {
                Id = 1,
                Email = loginDto.Email,
                PasswordHash = "hashed_password",
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Age = 25,
                MonthlyIncome = 5000,
                IsBlocked = false,
                Role = "User"
            };

            _userRepositoryMock.Setup(x => x.GetByEmail(loginDto.Email)).Returns(user);
            _passwordHasherMock.Setup(x => x.Verify(loginDto.Password, user.PasswordHash)).Returns(true);
            _tokenServiceMock.Setup(x => x.GenerateToken(user)).Returns("test_token");

            // Act
            var result = _authService.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test_token", result.Token);
            Assert.NotNull(result.User);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            _userRepositoryMock.Setup(x => x.GetByEmail(loginDto.Email)).Returns((User)null);

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => _authService.Login(loginDto));
        }
    }
}


