using SmartScheduler.WPF.Models;

namespace SmartScheduler.WPF.Session
{
    public static class SessionManager
    {
        public static User LoggedInUser { get; private set; }

        public static void SetUser(User user)
        {
            LoggedInUser = user;
        }

        public static void Clear()
        {
            LoggedInUser = null;
        }

        public static bool IsLoggedIn => LoggedInUser != null;
    }
}