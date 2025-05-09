using SmartScheduler.WPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Wpf.Ui.Input;
using SmartScheduler.WPF.Models;

namespace SmartScheduler.WPF.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        #region props + boilerplate
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirm = string.Empty;
        private bool _isSubmitted;
        public bool IsSubmitted
        {
            get => _isSubmitted;
            set { _isSubmitted = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username; set { _username = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get => _email; set { _email = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password; set { _password = value; OnPropertyChanged(); }
        }
        public string ConfirmPassword
        {
            get => _confirm; set { _confirm = value; OnPropertyChanged(); }
        }
        #endregion

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(_ => ExecuteRegister());
        }

        private void ExecuteRegister()
        {
            IsSubmitted = true;
            // 0. trim & null‑check
            string u = Username.Trim();
            string em = Email.Trim();
            string pw = Password;
            string cp = ConfirmPassword;

            // 1. câmpuri goale
            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(em) || string.IsNullOrEmpty(pw))
            {
                Show("Completaţi toate câmpurile!"); return;
            }

            // 2. email valid
            if (!ValidationHelper.IsEmail(em))
            {
                Show("E‑mail invalid!"); return;
            }

            // 3. parolă puternică
            if (!ValidationHelper.IsStrongPwd(pw))
            {
                Show("Parola trebuie să aibă min. 8 caractere, " +
                     "cel puţin o literă mare, o literă mică, o cifră şi un simbol.");
                return;
            }

            // 4. confirmare parolă
            if (pw != cp)
            {
                Show("Parolele nu coincid!"); return;
            }

            // 5. apel UserService
            var svc = UserService.GetInstance();
            var created = svc.RegisterUser(u, em, pw);
            if (created == null)
            {
                Show("Numele de utilizator este deja folosit."); return;
            }

            Show("Cont creat cu succes!", MessageBoxImage.Information);
            ClearFields();
            IsSubmitted = false;
        }

        private static void Show(string msg, MessageBoxImage img = MessageBoxImage.Warning) =>
            MessageBox.Show(msg, "Înregistrare", MessageBoxButton.OK, img);

        private void ClearFields()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}
