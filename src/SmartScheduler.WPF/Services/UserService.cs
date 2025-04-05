using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Repository.Implementations;
using SmartScheduler.WPF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Services
{
    public class UserService
    {
        private static UserService? _instance;
        private readonly IUserRepository _userRepository;

        private UserService()
        {
            _userRepository = new UserRepository();
        }

        // Metoda statică pentru obținerea instanței
        public static UserService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserService();
            }
            return _instance;
        }

        /// <summary>
        ///  Înregistrează un nou utilizator, hash-uind parola și salvând userul în DB.
        ///  Returnează user-ul creat sau null dacă userul cu același username deja există.
        /// </summary>
        public User? RegisterUser(string username, string email, string password)
        {
            // 1. Verificăm dacă există deja un user cu acest username
            var existingUser = _userRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                // Deja există un user cu acest nume => returnăm null sau aruncăm o excepție
                return null;
            }

            // 2. Hash-uim parola cu BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // 3. Creăm obiectul UserModel
            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                FreeTimeIntervals = new List<FreeTimeInterval>(),
                Tasks = new List<TaskModel>()
            };

            // 4. Salvăm în DB
            var createdUser = _userRepository.CreateUser(newUser);
            return createdUser;
        }

        /// <summary>
        ///  Autentifică un utilizator pe baza username-ului și parolei.
        ///  Returnează user-ul dacă parola este corectă, altfel null.
        /// </summary>
        public User? LoginUser(string username, string password)
        {
            // 1. Căutăm userul după username
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                // Nu există user => login eșuat
                return null;
            }

            // 2. Verificăm parola prin BCrypt
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!passwordMatches)
            {
                // Parola incorectă
                return null;
            }

            // Dacă parola e corectă, returnăm user-ul
            return user;
        }

        /// <summary>
        ///  Actualizează un utilizator (de ex. modifică emailul, username-ul, etc.).
        ///  Nu schimbă parola! (Dacă vrei să schimbi parola, creează o metodă separată.)
        /// </summary>
        public User UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        /// <summary>
        ///  Șterge user-ul (by ID). Returnează true dacă s-a șters cu succes, altfel false.
        /// </summary>
        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }
    }
}
