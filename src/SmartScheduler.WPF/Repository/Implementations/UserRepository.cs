using Microsoft.EntityFrameworkCore;
using SmartScheduler.WPF.Models;
using System.Linq;

namespace SmartScheduler.WPF.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        SmartSchedulerContext _context;

        public UserRepository()
        {
            _context = SmartSchedulerContext.GetInstance();
        }

        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User? GetUserById(int userId)
        {
            return _context.Users
                .Include(u => u.Tasks)
                .Include(u => u.FreeTimeIntervals)
                .FirstOrDefault(u => u.Id == userId);
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users
                .Include(u => u.Tasks)
                .Include(u => u.FreeTimeIntervals)
                .FirstOrDefault(u => u.Username == username);
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Tasks)
                .Include(u => u.FreeTimeIntervals)
                .FirstOrDefault(u => u.Email == email);
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();

            return true;
        }
    }
}
