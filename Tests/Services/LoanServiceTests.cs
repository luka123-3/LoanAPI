using Application;
using Application.DTOs.Loan;
using Application.Services;
using Infrastructure;
using Domain;
using Domain.Enums;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class LoanServiceTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LoanService _loanService;

        public LoanServiceTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loanService = new LoanService(
                _loanRepositoryMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public void CreateLoan_WithValidData_ReturnsLoanDto()
        {
            // Arrange
            var userId = 1;
            var createLoanDto = new CreateLoanDto
            {
                Type = LoanType.QuickLoan,
                Amount = 1000,
                Currency = "USD",
                DurationMonths = 12
            };

            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsBlocked = false
            };

            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(user);

            // Act
            var result = _loanService.CreateLoan(userId, createLoanDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createLoanDto.Amount, result.Amount);
            Assert.Equal(createLoanDto.Currency, result.Currency);
            Assert.Equal(LoanStatus.Processing, result.Status);
            _loanRepositoryMock.Verify(x => x.Add(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public void CreateLoan_WithBlockedUser_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var createLoanDto = new CreateLoanDto
            {
                Type = LoanType.QuickLoan,
                Amount = 1000,
                Currency = "USD",
                DurationMonths = 12
            };

            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                IsBlocked = true
            };

            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(user);

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => _loanService.CreateLoan(userId, createLoanDto));
        }

        [Fact]
        public void UpdateMyLoan_WithProcessingStatus_UpdatesLoan()
        {
            // Arrange
            var userId = 1;
            var loanId = 1;
            var updateLoanDto = new UpdateLoanDto
            {
                Amount = 2000,
                Currency = "EUR"
            };

            var loan = new Loan
            {
                Id = loanId,
                UserId = userId,
                Amount = 1000,
                Currency = "USD",
                Status = LoanStatus.Processing
            };

            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe"
            };

            _loanRepositoryMock.Setup(x => x.GetById(loanId)).Returns(loan);
            _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(user);

            // Act
            var result = _loanService.UpdateMyLoan(userId, loanId, updateLoanDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateLoanDto.Amount, result.Amount);
            _loanRepositoryMock.Verify(x => x.Update(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public void UpdateMyLoan_WithNonProcessingStatus_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var loanId = 1;
            var updateLoanDto = new UpdateLoanDto { Amount = 2000 };

            var loan = new Loan
            {
                Id = loanId,
                UserId = userId,
                Status = LoanStatus.Approved
            };

            _loanRepositoryMock.Setup(x => x.GetById(loanId)).Returns(loan);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _loanService.UpdateMyLoan(userId, loanId, updateLoanDto));
        }
    }
}


