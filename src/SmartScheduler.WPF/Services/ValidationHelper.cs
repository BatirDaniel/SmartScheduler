using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Services
{
    internal static class ValidationHelper
    {
        // regex simplu pentru RFC 5322 basic
        private static readonly Regex _emailRx = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // minim 8; 1 majusculă, 1 minusculă, 1 cifră, 1 simbol
        private static readonly Regex _pwdRx = new(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            RegexOptions.Compiled);

        public static bool IsEmail(string? s) => s != null && _emailRx.IsMatch(s);
        public static bool IsStrongPwd(string? s) => s != null && _pwdRx.IsMatch(s);
    }
}
