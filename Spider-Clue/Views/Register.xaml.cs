using Spider_Clue.SpiderClueService;
using System;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Controls;
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
                this.NavigationService.GoBack();
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
                if (!VerifyDuplications())
                {
                    registerResult = RegisterGamerInDatabase();
                }
            }

            return registerResult;
        }


        private bool IsEmailVerified()
        {
            String toEmail = txtEmail.Text;
            return Utilities.SendEmailWithCode(toEmail, Window.GetWindow(this));
            
        }

        private bool AreDataValid()
        {
            bool accountDataValid = ValidateAccountData();
            bool userDataValid = ValidateUserData();
            bool passwordsMatch = ArePasswordsMatching();
            bool emailVerified = IsEmailVerified();

            return accountDataValid && userDataValid && passwordsMatch && emailVerified;
        }

        private bool ValidateAccountData()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string email = txtEmail.Text;

            bool passwordValid = Validations.IsPasswordValid(password);
            bool emailValid = Validations.IsEmailValid(email);

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
        private bool ValidateUserData()
        {
            string gamerTag = txtGamerTag.Text;
            string name = txtName.Text;
            string lastName = txtLastName.Text;
            bool nameValid = Validations.IsNameValid(name);
            bool lastNameValid = Validations.IsNameValid(lastName);
            bool gamerTagValid = Validations.IsGamerTagValid(gamerTag);

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
        private bool ArePasswordsMatching()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            SecureString securePasswordToConfirm = txtConfirmPassword.SecurePassword;
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
            Utilities.PlayButtonClickSound();
            this.NavigationService.GoBack();
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
            string defaultIcon = "Icon0.jpg";
            bool result = false;
            string passwordHashed = Utilities.CalculateSHA1Hash(txtPassword.Password);
            Gamer gamer = new Gamer()
            {
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                Gamertag = txtGamerTag.Text,
                Email = txtEmail.Text,
                Password = passwordHashed,
                ImageCode = defaultIcon,
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