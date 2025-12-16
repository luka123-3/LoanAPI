using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUserService
    {
        UserDto GetUserById(int id);
        UserDto BlockUser(int userId, DateTime? blockedUntil = null);
        UserDto UnblockUser(int userId);
        bool IsUserBlocked(int userId);
    }
}
