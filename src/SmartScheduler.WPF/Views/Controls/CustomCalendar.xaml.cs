using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SmartScheduler.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for CustomCalendar.xaml
    /// </summary>
    public partial class CustomCalendar : UserControl
    {
        public CustomCalendar()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            InnerCalendar.DisplayDate = InnerCalendar.DisplayDate.AddMonths(-1);
            UpdateTitle();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            InnerCalendar.DisplayDate = InnerCalendar.DisplayDate.AddMonths(1);
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            TitleText.Text = InnerCalendar.DisplayDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
        }
    }
}