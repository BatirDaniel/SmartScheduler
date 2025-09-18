using System.Windows.Controls;
using System.Windows.Input;

namespace SmartScheduler.WPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegisterView registerPage = new RegisterView();
            NavigationService?.Navigate(registerPage);
        }
    }
}