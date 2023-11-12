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
        public Validations() { }

        public static bool IsPasswordValid(string password)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
            }
            else
            {
                Regex passwordRegex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d\\W]{8,50}$");

                if (!passwordRegex.IsMatch(password))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        public static bool IsEmailValid(string email)
        {
            bool emailValidation = true;

            if (string.IsNullOrEmpty(email) || email.Length > 50)
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
