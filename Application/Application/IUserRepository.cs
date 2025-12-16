using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByEmail(string email);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}
