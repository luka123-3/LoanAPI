using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public LoanType Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int DurationMonths { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
