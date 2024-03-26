using System.Net;
using System.Security;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.Windows;
using System.ServiceModel;
using System;

namespace Spider_Clue.Views
{
    public partial class PasswordRecoveryView : Page
    {
        private string gamertag;
        public PasswordRecoveryView()
        {
            InitializeComponent();
        }

        public void SetGamertagInWindow(string gamertag)
        {
            this.gamertag = gamertag;
        }

        private bool ValidatePassword()
        {
            bool passwordValid = Validations.ValidatePassword(pwbPassword.SecurePassword);
            bool passwordsMatching = Validations.ArePasswordsMatching(pwbPassword.SecurePassword, pwbConfirmPassword.SecurePassword);

            if (!passwordValid)
            {
                lblPasswordInvalid.Visibility = Visibility.Visible;
            }

            if (!passwordsMatching)
            {
                lblPasswordsDontMatch.Visibility = Visibility.Visible;
            }

            return passwordValid && passwordsMatching;
        }

        private void BtnConfirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();

            if (ValidatePassword())
            {
                if (UpdateGamerPassword())
                {
                    DialogManager.ShowSuccessMessageBox(Properties.Resources.DlgSuccessfulChange);
                    GoToLoginView();
                }
                else
                {
                    DialogManager.ShowWarningMessageBox(Properties.Resources.DlgWrongChange);
                }
            }
        }

        private void GoToLoginView()
        {
            Utilities.PlayButtonClickSound();
            LoginView loginView = new LoginView();
            this.NavigationService.Navigate(loginView);
        }

        private bool UpdateGamerPassword()
        {
            bool result = false;
            SecureString securePassword = pwbPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string newPassword = Utilities.CalculateSHA1Hash(password);
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();

                if (userManager.UpdatePassword(gamertag, newPassword) == 1)
                {
                    result = true;
                }
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                result = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                result = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                result = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                result = false;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
            return result;
        }

        private void BtnGoBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            this.NavigationService.GoBack();
        }

        private void TypingConfirmPassword(object sender, RoutedEventArgs e)
        {
            lblPasswordsDontMatch.Visibility = Visibility.Collapsed;
        }

        private void TypingPassword(object sender, RoutedEventArgs e)
        {
            lblPasswordInvalid.Visibility = Visibility.Collapsed;
        }
    }
}
