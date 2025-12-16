using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ILoanRepository
    {
        Loan GetById(int id);
        List<Loan> GetAll();
        List<Loan> GetByUserId(int userId);
        void Add(Loan loan);
        void Update(Loan loan);
        void Delete(int id);
    }
}
