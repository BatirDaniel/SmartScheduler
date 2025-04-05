using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Repository
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        User? GetUserById(int userId);
        User? GetUserByUsername(string username);
        User UpdateUser(User user);
        bool DeleteUser(int userId);
    }
}
