using Application;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public Loan GetById(int id)
        {
            return _context.Loans
                .Include(l => l.User)
                .FirstOrDefault(l => l.Id == id);
        }

        public List<Loan> GetAll()
        {
            return _context.Loans
                .Include(l => l.User)
                .ToList();
        }

        public List<Loan> GetByUserId(int userId)
        {
            return _context.Loans
                .Where(l => l.UserId == userId)
                .ToList();
        }

        public void Add(Loan loan)
        {
            _context.Loans.Add(loan);
            _context.SaveChanges();
        }

        public void Update(Loan loan)
        {
            _context.Loans.Update(loan);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var loan = GetById(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                _context.SaveChanges();
            }
        }
    }
}
