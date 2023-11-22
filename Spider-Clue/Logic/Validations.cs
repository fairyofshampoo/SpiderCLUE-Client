using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider_Clue.Logic
{
    internal class Validations
    {
        protected Validations() { }

        public static bool IsPasswordValid(string password)
        {
            bool isValid = true;
            int limitTime = 500;
            if (string.IsNullOrWhiteSpace(password))
        {
            isValid = false;
        }
        else
        {
            try
            {
                Regex passwordRegex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d\\W]{8,50}$", 
                    RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

                isValid = passwordRegex.IsMatch(password);
            }
            catch (RegexMatchTimeoutException)
            {
                isValid = false;
            }
        }

        return isValid;
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
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(name))
            {
                isValid = false;
            }
            else
            {
                var nameRegex = new Regex("^[\\p{L}\\p{M}\\s]{1,50}");

                if (!nameRegex.IsMatch(name))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        public static bool IsGamerTagValid(string gamerTag)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(gamerTag))
            {
                isValid = false;
            }
            else
            {
                var gamerTagRegex = new Regex("^[A-Za-z0-9]{1,15}");

                if (!gamerTagRegex.IsMatch(gamerTag))
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
