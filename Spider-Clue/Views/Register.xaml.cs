using Microsoft.Win32;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using Spider_Clue.SpiderClueService;
using System.Net.Mail;

namespace Spider_Clue.Views
{
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (AreDataValid())
            {
                if (RegisterGamerInDatabase())
                {
                    MessageBox.Show("Exito", "EXITO", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Ocurrió algo feito", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AreDataValid()
        {
            bool dataValidation = false;

            if(ValidateAccountData() && ValidateUserData() && ArePasswordsMatching())
            {
                dataValidation = true;
            }


            return dataValidation;
        }

        private bool ValidateAccountData()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string email = txtEmail.Text;
            Regex passwordRegex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d\\W]{8,50}$");
            bool dataValidation = true;

            if (!passwordRegex.IsMatch(password))
            {
                dataValidation = false;
                lblInvalidPassword.Visibility = Visibility.Visible;
            }

            if (!IsValidEmail(email))
            {
                dataValidation = false;
                lblInvalidEmail.Visibility = Visibility.Visible;
            }

            return dataValidation;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length > 50)
            {
                return false;
            }
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool ValidateUserData()
        {
            string gamerTag = txtGamerTag.Text;
            string name = txtName.Text;
            string lastName = txtLastName.Text;
            Regex nameRegex = new Regex("^[\\p{L}\\p{M}\\s]{1,50}$");
            Regex gamerTagRegex = new Regex("^(?=.*[A-Za-z0-9])[A-Za-z0-9]{1,15}$");
            bool dataValidation = true;

            if (!nameRegex.IsMatch(name))
            {
                dataValidation = false;
                lblInvalidName.Visibility = Visibility.Visible;
            }

            if (!nameRegex.IsMatch(lastName))
            {
                dataValidation = false;
                lblInvalidLastName.Visibility = Visibility.Visible;
            }

            if (!gamerTagRegex.IsMatch(gamerTag))
            {
                dataValidation = false;
                lblInvalidUserName.Visibility = Visibility.Visible;
            }

            return dataValidation;
        }

        private bool ArePasswordsMatching()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            SecureString securePasswordToConfirm = txtConfirmPasssword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string passwordToConfirm = new NetworkCredential(string.Empty, securePasswordToConfirm).Password;
            bool passwordsValidation = false;

            if(password.Equals(passwordToConfirm))
            {
                passwordsValidation = true;
            } 
            else
            {
                lblPasswordsDontMatch.Visibility = Visibility.Visible;
            }

            return passwordsValidation;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            this.NavigationService.Navigate(loginView);
        }

        public static string CalculateSHA1Hash(string textToHash)
        {
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(textToHash);
                byte[] hash = sha1.ComputeHash(bytes);

                StringBuilder textToHashBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    textToHashBuilder.Append(hash[i].ToString("x2"));
                }

                return textToHashBuilder.ToString();
            }
        }

        private Boolean RegisterGamerInDatabase()
        {
            bool result = false;
            Gamer gamer = new Gamer()
            {
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                Gamertag = txtGamerTag.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Password,
            };
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            if(userManager.AddUserTransaction(gamer) == 1)
            {
                result = true;
            }
            return result;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
           
                Gamer gamer = new Gamer()
                {
                    FirstName = txtName.Text,
                    LastName = txtLastName.Text,
                    Gamertag = txtGamerTag.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                };
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                userManager.AddUserTransaction(gamer);
        }
    }
}
