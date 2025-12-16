using Application.DTOs.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ILoanService
    {
        // User
        LoanDto CreateLoan(int userId, CreateLoanDto dto);
        List<LoanDto> GetMyLoans(int userId);
        LoanDto UpdateMyLoan(int userId, int loanId, UpdateLoanDto dto);
        bool DeleteMyLoan(int userId, int loanId);

        // Accountant
        LoanDto GetLoanById(int loanId);
        List<LoanDto> GetAllLoans();
        LoanDto UpdateLoanStatus(int loanId, UpdateLoanStatusDto dto);
        LoanDto UpdateLoan(int loanId, UpdateLoanDto dto);
        bool DeleteLoan(int loanId);
    }
}
