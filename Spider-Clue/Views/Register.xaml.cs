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
using System.Net.Mail;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;
using Spider_Clue.Logic;

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
            if (RegisterUser())
            {
                ShowSuccessMessage();
            }
            else
            {
                ShowErrorMessage();
            }
        }

        private void ShowSuccessMessage()
        {
            MessageBox.Show(Properties.Resources.DlgRegisterSuccessful, Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show(Properties.Resources.DlgRegisterError, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private bool RegisterUser()
        {
            bool registerResult = false;

            if (AreDataValid())
            {
                if (IsEmailVerified())
                {
                    if (!VerifyDuplications()) 
                    {
                        registerResult = RegisterGamerInDatabase();
                    }
                }
            }

            return registerResult;
        }


        private bool IsEmailVerified()
        {
            String toEmail = txtEmail.Text;
            SpiderClueService.IEmailVerificationManager emailVerificationManager = new SpiderClueService.EmailVerificationManagerClient();
            emailVerificationManager.GenerateVerificationCode(toEmail);
            String codeToValidate = OpenDialogForEmailVerification();
            return emailVerificationManager.VerifyCode(toEmail, codeToValidate);
        }

        private string OpenDialogForEmailVerification()
        {
            Window mainWindow = Window.GetWindow(this);

            CodeInputDialog codeInputPopUp = new CodeInputDialog();
            codeInputPopUp.Owner = mainWindow;

            string codeFromInput = null;

            if (codeInputPopUp.ShowDialog() == true)
            {
                codeFromInput = codeInputPopUp.EmailValidation;
            }
            return codeFromInput;
        }


        private bool AreDataValid()
        {
            bool accountDataValid = ValidateAccountData();
            bool userDataValid = ValidateUserData();
            bool passwordsMatch = ArePasswordsMatching();

            return accountDataValid && userDataValid && passwordsMatch;
        }

        private bool ValidateAccountData()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string email = txtEmail.Text;

            bool passwordValid = IsPasswordValid(password);
            bool emailValid = IsEmailValid(email);

            if (!passwordValid)
            {
                lblInvalidPassword.Visibility = Visibility.Visible;
            }

            if (!emailValid)
            {
                lblInvalidEmail.Visibility = Visibility.Visible;
            }

            return passwordValid && emailValid;
        }

        private bool IsPasswordValid(string password)
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

        private bool IsEmailValid(string email)
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


        private bool ValidateUserData()
        {
            string gamerTag = txtGamerTag.Text;
            string name = txtName.Text;
            string lastName = txtLastName.Text;
            bool nameValid = IsNameValid(name);
            bool lastNameValid = IsNameValid(lastName);
            bool gamerTagValid = IsGamerTagValid(gamerTag);

            if (!nameValid)
            {
                lblInvalidName.Visibility = Visibility.Visible;
            }

            if (!lastNameValid)
            {
                lblInvalidLastName.Visibility = Visibility.Visible;
            }

            if (!gamerTagValid)
            {
                lblInvalidGamerTag.Visibility = Visibility.Visible;
            }

            return nameValid && lastNameValid && gamerTagValid;
        }

        private bool IsNameValid(string name)
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


        private bool IsGamerTagValid(string gamerTag)
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


        private bool ArePasswordsMatching()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            SecureString securePasswordToConfirm = txtConfirmPasssword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string passwordToConfirm = new NetworkCredential(string.Empty, securePasswordToConfirm).Password;
            bool passwordsValidation = false;

            if (!string.IsNullOrWhiteSpace(password) || !string.IsNullOrWhiteSpace(passwordToConfirm))
            {
                if (string.Equals(password, passwordToConfirm))
                {
                    passwordsValidation = true;
                }
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

        private bool VerifyDuplications()
        {
            bool emailDuplication = SearchEmailDuplication();
            bool gamerTagDuplication = SearchGamerTagDuplication();

            return emailDuplication || gamerTagDuplication;
        }

        private bool SearchEmailDuplication()
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            bool emailDuplication = userManager.IsEmailExisting(txtEmail.Text);
            if (emailDuplication)
            {
                lblInvalidEmail.Visibility = Visibility.Visible;
            }
            return emailDuplication;
        }

        private bool SearchGamerTagDuplication()
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            bool gamerTagDuplication = userManager.IsGamertagExisting(txtGamerTag.Text);
            if (gamerTagDuplication)
            {
                lblInvalidGamerTag.Visibility = Visibility.Visible;
            }

            return gamerTagDuplication;

        }

        private Boolean RegisterGamerInDatabase()
        {
            bool result = false;
            string passwordHashed = HashUtility.CalculateSHA1Hash(txtPassword.Password);
            Gamer gamer = new Gamer()
            {
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                Gamertag = txtGamerTag.Text,
                Email = txtEmail.Text,
                Password = passwordHashed,
            };
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            if (userManager.AddUserTransaction(gamer) == 1)
            {
                result = true;
            }
            return result;
        }

        private void TypingName(object sender, TextChangedEventArgs e)
        {
            lblInvalidName.Visibility = Visibility.Hidden;
        }

        private void TypingLastName(object sender, TextChangedEventArgs e)
        {
            lblInvalidLastName.Visibility = Visibility.Hidden;
        }

        private void TypingGamerTag(object sender, TextChangedEventArgs e)
        {
            lblInvalidGamerTag.Visibility = Visibility.Hidden;
        }

        private void TypingEMail(object sender, TextChangedEventArgs e)
        {
            lblInvalidEmail.Visibility = Visibility.Hidden;
        }

        private void TypingPassword(object sender, RoutedEventArgs e)
        {
            lblInvalidPassword.Visibility = Visibility.Hidden;
        }

        private void TypingPasswordToConfirm(object sender, RoutedEventArgs e)
        {
            lblPasswordsDontMatch.Visibility = Visibility.Hidden;
        }
    }
}