using System.Windows;
using System.Windows.Controls;

namespace SmartScheduler.WPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();

            MainFrame.Navigate(new DashboardView());
        }

        private void dashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardView());
        }

        private void tasksBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TasksView());
        }

        private void freeTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FreeTimeView());
        }

        private void hobbiesBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HobbiesView());
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsView());
        }

        private void profileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfileView());
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LoginView());
        }
    }
}