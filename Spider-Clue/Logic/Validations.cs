using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider_Clue.Logic
{
    internal class Validations
    {
        protected Validations() { }

        private static bool ValidateWithTimeout(string input, Regex regex)
        {
            bool isValid;

            try
            {
                isValid = regex.IsMatch(input);
            }
            catch (RegexMatchTimeoutException)
            {
                isValid = false;
            }

            return isValid;
        }

        public static bool IsPasswordValid(string password)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
            }

            var passwordRegex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d\\W]{8,50}$",
                                         RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(password, passwordRegex);
        }

        public static bool IsMessageValid(string message)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(message))
            {
                isValid = false;
            }

            var messageRegex = new Regex("^[\\p{L}\\d\\s\\W]{0,200}$",
                                         RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(message, messageRegex);
        }

        public static bool IsEmailValid(string email)
        {
            bool emailValidation = true;
            int maximumEmailLength = 50;

            if (string.IsNullOrEmpty(email) || email.Length > maximumEmailLength)
            {
                emailValidation = false;
            }
            else
            {
                try
                {
                    var mailAddress = new MailAddress(email);
                }
                catch (FormatException)
                {
                    emailValidation = false;
                }
            }

            return emailValidation;
        }

        public static bool IsNameValid(string name)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(name))
            {
                isValid = false;
            }

            var nameRegex = new Regex("^[\\p{L}\\p{M}\\s]{1,50}",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(name, nameRegex);
        }

        public static bool IsGamerTagValid(string gamerTag)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(gamerTag))
            {
                isValid = false;
            }
            var gamerTagRegex = new Regex("^[A-Za-z0-9]{1,15}",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(gamerTag, gamerTagRegex);
        }

        public static bool ValidatePassword(SecureString securePassword)
        {
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            return IsPasswordValid(password);
        }

        public static bool ArePasswordsMatching(SecureString password, SecureString passwordToConfirm)
        {
            string plainPassword = new NetworkCredential(string.Empty, password).Password;
            string plainPasswordToConfirm = new NetworkCredential(string.Empty, passwordToConfirm).Password;

            return string.Equals(plainPassword, plainPasswordToConfirm);
        }
    }
}
