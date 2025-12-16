using Application.DTOs.Loan;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Enums;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUserRepository _userRepository;

        public LoanService(ILoanRepository loanRepository, IUserRepository userRepository)
        {
            _loanRepository = loanRepository;
            _userRepository = userRepository;
        }

        public LoanDto CreateLoan(int userId, CreateLoanDto dto)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (user.IsBlocked)
            {
                if (user.BlockedUntil.HasValue && user.BlockedUntil.Value < DateTime.UtcNow)
                {
                    user.IsBlocked = false;
                    user.BlockedUntil = null;
                    _userRepository.Update(user);
                }
                else
                {
                    throw new UnauthorizedAccessException("User is blocked and cannot request loans");
                }
            }

            var loan = new Loan
            {
                UserId = userId,
                Type = dto.Type,
                Amount = dto.Amount,
                Currency = dto.Currency,
                DurationMonths = dto.DurationMonths,
                Status = LoanStatus.Processing,
                CreatedAt = DateTime.UtcNow
            };

            _loanRepository.Add(loan);
            return MapToDto(loan, user);
        }

        public List<LoanDto> GetMyLoans(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var loans = _loanRepository.GetByUserId(userId);
            return loans.Select(loan => MapToDto(loan, user)).ToList();
        }

        public LoanDto UpdateMyLoan(int userId, int loanId, UpdateLoanDto dto)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found");
            }

            if (loan.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't have permission to update this loan");
            }

            if (loan.Status != LoanStatus.Processing)
            {
                throw new ArgumentException("Only loans with Processing status can be updated");
            }

            if (dto.Amount.HasValue)
                loan.Amount = dto.Amount.Value;

            if (!string.IsNullOrEmpty(dto.Currency))
                loan.Currency = dto.Currency;

            if (dto.DurationMonths.HasValue)
                loan.DurationMonths = dto.DurationMonths.Value;

            _loanRepository.Update(loan);
            var user = _userRepository.GetById(userId);
            return MapToDto(loan, user);
        }

        public bool DeleteMyLoan(int userId, int loanId)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found");
            }

            if (loan.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't have permission to delete this loan");
            }

            if (loan.Status != LoanStatus.Processing)
            {
                throw new ArgumentException("Only loans with Processing status can be deleted");
            }

            _loanRepository.Delete(loanId);
            return true;
        }

        public LoanDto GetLoanById(int loanId)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found");
            }

            var user = _userRepository.GetById(loan.UserId);
            return MapToDto(loan, user);
        }

        public List<LoanDto> GetAllLoans()
        {
            var loans = _loanRepository.GetAll();
            return loans.Select(loan =>
            {
                var user = _userRepository.GetById(loan.UserId);
                return MapToDto(loan, user);
            }).ToList();
        }

        public LoanDto UpdateLoanStatus(int loanId, UpdateLoanStatusDto dto)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found");
            }

            loan.Status = dto.Status;
            _loanRepository.Update(loan);

            var user = _userRepository.GetById(loan.UserId);
            return MapToDto(loan, user);
        }

        public LoanDto UpdateLoan(int loanId, UpdateLoanDto dto)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found");
            }

            if (dto.Amount.HasValue)
                loan.Amount = dto.Amount.Value;

            if (!string.IsNullOrEmpty(dto.Currency))
                loan.Currency = dto.Currency;

            if (dto.DurationMonths.HasValue)
                loan.DurationMonths = dto.DurationMonths.Value;

            _loanRepository.Update(loan);
            var user = _userRepository.GetById(loan.UserId);
            return MapToDto(loan, user);
        }

        public bool DeleteLoan(int loanId)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new KeyNotFoundException("Loan not found");
            }

            _loanRepository.Delete(loanId);
            return true;
        }

        private LoanDto MapToDto(Loan loan, User user)
        {
            return new LoanDto
            {
                Id = loan.Id,
                UserId = loan.UserId,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Type = loan.Type,
                Amount = loan.Amount,
                Currency = loan.Currency,
                DurationMonths = loan.DurationMonths,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt
            };
        }
    }
}
