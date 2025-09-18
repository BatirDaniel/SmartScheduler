using System.Windows;
using System.Windows.Controls;

namespace SmartScheduler.WPF.Services
{
    public class NavigationService
    {
        private static readonly NavigationService _inst = new();

        private NavigationService()
        { }

        public static NavigationService Inst => _inst;

        public void Show(Page page)
        {
            if (Application.Current.MainWindow!.Content is System.Windows.Controls.Page p &&
                p.NavigationService != null)
            {
                // suntem într‑un NavigationWindow / Frame
                p.NavigationService.Navigate(page);
            }
            else
            {
                // page direct în Window.Content
                Application.Current.MainWindow!.Content = page;
            }
        }
    }
}