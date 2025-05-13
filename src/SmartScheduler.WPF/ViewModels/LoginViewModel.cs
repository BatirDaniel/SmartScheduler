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
using SmartScheduler.WPF.Models;
using SmartScheduler.WPF.Views.Pages;

namespace SmartScheduler.WPF.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email = string.Empty;
        private string _password = string.Empty;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => ExecuteLogin());
        }

        private void ExecuteLogin()
        {
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Introduceţi e‑mail şi parolă!",
                                "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var userService = UserService.GetInstance();
            User? user = userService.LoginUserByEmail(Email.Trim(), Password);

            if (user == null)
            {
                MessageBox.Show("E‑mail sau parolă incorectă!",
                                "Autentificare eşuată", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

          
            if (user != null)
            {
                NavigationService.Inst.Show(new DashboardView());
                ClearFields();
            }

            ClearFields();
        }

        private void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? p = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        #endregion
    }
}
