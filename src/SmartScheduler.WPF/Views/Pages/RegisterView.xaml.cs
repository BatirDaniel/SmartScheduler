using System.Windows.Controls;
using System.Windows.Input;

namespace SmartScheduler.WPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Page
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoginView loginPage = new LoginView();
            NavigationService?.Navigate(loginPage);
        }
    }
}