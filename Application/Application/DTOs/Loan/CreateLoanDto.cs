using Domain;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Loan
{
    public class CreateLoanDto
    {
        public LoanType Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int DurationMonths { get; set; }
    }
}
