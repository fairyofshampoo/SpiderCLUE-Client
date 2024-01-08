using Spider_Clue.SpiderClueService;
using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.ServiceModel;

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
            Utilities.PlayButtonClickSound();

            if (RegisterUser())
            {
                DialogManager.ShowSuccessMessageBox(Properties.Resources.DlgRegisterSuccessful);
                GoToLoginView();
            }
            else
            {
                DialogManager.ShowWarningMessageBox(Properties.Resources.DlgRegisterError);
            }
        }

        private bool RegisterUser()
        {
            bool registerResult = false;

            if (AreDataValid() && IsEmailVerified())
            {
                registerResult = RegisterGamerInDatabase();
            }
            return registerResult;
        }

        private bool IsEmailVerified()
        {
            string toEmail = txtEmail.Text;
            return Utilities.SendEmailWithCode(toEmail, Window.GetWindow(this));
        }

        private bool AreDataValid()
        {
            bool accountDataValid = ValidateAccountData();
            bool userDataValid = ValidateUserData();
            bool passwordsMatch = ArePasswordsMatching();
            bool duplicationValidation = VerifyDuplications();
            return accountDataValid && userDataValid && passwordsMatch && !duplicationValidation;
        }

        private bool ValidateAccountData()
        {
            SecureString securePassword = pwbPassword.SecurePassword;
            string email = txtEmail.Text;

            bool passwordValid = Validations.ValidatePassword(securePassword);
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
            SecureString securePassword = pwbPassword.SecurePassword;
            SecureString securePasswordToConfirm = txtConfirmPassword.SecurePassword;
            
            bool passwordsValidation = false;
            if (Validations.ValidatePassword(securePassword))
            {
                if (Validations.ArePasswordsMatching(securePassword, securePasswordToConfirm))
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
            GoToLoginView();
        }

        private void GoToLoginView()
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
            bool emailDuplication = false;
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                emailDuplication = userManager.IsEmailExisting(txtEmail.Text);
                if (emailDuplication)
                {
                    lblInvalidEmail.Content = Properties.Resources.LblEmailUsed;
                    lblInvalidEmail.Visibility = Visibility.Visible;
                }
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                emailDuplication = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                emailDuplication = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                emailDuplication = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                emailDuplication = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }


            return emailDuplication;
        }

        private bool SearchGamerTagDuplication()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            bool gamerTagDuplication = false;

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                gamerTagDuplication = userManager.IsGamertagExisting(txtGamerTag.Text);
                if (gamerTagDuplication)
                {
                    lblInvalidGamerTag.Content = Properties.Resources.LblGamerTagUsed;
                    lblInvalidGamerTag.Visibility = Visibility.Visible;
                }
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
                gamerTagDuplication = false;
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
                gamerTagDuplication = false;
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
                gamerTagDuplication = false;
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
                gamerTagDuplication = false;
            }

            return gamerTagDuplication;
        }

        private bool RegisterGamerInDatabase()
        {
            string defaultIcon = "Icon0.jpg";
            bool result = false;
            string passwordHashed = Utilities.CalculateSHA1Hash(pwbPassword.Password);
            Gamer gamer = new Gamer()
            {
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                Gamertag = txtGamerTag.Text,
                Email = txtEmail.Text,
                Password = passwordHashed,
                ImageCode = defaultIcon,
            };
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                if (userManager.AddUserTransaction(gamer) == 1)
                {
                    result = true;
                }
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
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
            lblInvalidGamerTag.Content = Properties.Resources.LblInvalidGamerTag;
            lblInvalidGamerTag.Visibility = Visibility.Hidden;
        }

        private void TypingEMail(object sender, TextChangedEventArgs e)
        {
            lblInvalidGamerTag.Content = Properties.Resources.LblInvalidEMail;
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