using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public decimal MonthlyIncome { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? BlockedUntil { get; set; }
        public string Role { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
