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

namespace Spider_Clue.Views
{
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            this.NavigationService.Navigate(loginView);
        }

        private bool AreDataValid()
        {
            bool dataValidation = ValidateAccountData() && ValidateUserData();

            if (dataValidation)
            {
                dataValidation = ArePasswordsMatching();

                if (dataValidation)
                {
                    dataValidation = IsEmailExisting();
                }
            }

            return dataValidation;
        }

        private bool ValidateUserData()
        {
            string gamerTag = txtGamerTag.Text;
            string name = txtName.Text;
            string lastName = txtLastName.Text;
            Regex nameRegex = new Regex("^(?!.*\\s)(?=[a-zA-ZáéíóúüñÁÉÍÓÚÜÑ]+$).{1,50}$");
            Regex gamerTagRegex = new Regex("^(?=.*[A-Za-z0-9])[A-Za-z0-9]{1,15}$");
            bool dataValidation = true;

            if (!nameRegex.IsMatch(name))
            {
                dataValidation = false;
            }

            if (!nameRegex.IsMatch(lastName))
            {
                dataValidation = false;
            }

            if (!gamerTagRegex.IsMatch(gamerTag))
            {
                dataValidation = false;
            }

            return dataValidation;
        }


        private bool ValidateAccountData()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            SecureString securePasswordToConfirm = txtConfirmPasssword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string passwordToConfirm = new NetworkCredential(string.Empty, securePasswordToConfirm).Password;
            string email = txtEmail.Text;
            Regex passwordRegex = new Regex("^(?=.*[!@#$%^&*()_+])[A-Za-z0-9!@#$%^&*()_+]{8,16}$\r\n");
            Regex emailRegex = new Regex("^(?=.{1,50}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n");
            bool dataValidation = true;

            if (!passwordRegex.IsMatch(password))
            {
                dataValidation = false;
            }

            if (!passwordRegex.IsMatch(passwordToConfirm))
            {
                dataValidation=false;
            }

            if (!emailRegex.IsMatch(email))
            {
                dataValidation = false;
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

            if(password == passwordToConfirm)
            {
                passwordsValidation = true;
            } else
            {

            }

            return passwordsValidation;
        }

        private bool IsEmailExisting()
        {
            return true;
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
