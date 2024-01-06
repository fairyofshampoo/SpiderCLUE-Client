using Spider_Clue.SpiderClueService;
using System.Net;
using System.Security;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.Windows;

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
            bool passwordValid = Validations.ValidatePassword(txtPassword.SecurePassword);
            bool passwordsMatching = Validations.ArePasswordsMatching(txtPassword.SecurePassword, txtConfirmPassword.SecurePassword);

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
                    ShowSuccessMessage();
                    GoToLoginView();
                }
                else
                {
                    ShowErrorMessage();
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
            SecureString securePassword = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            string newPassword = Utilities.CalculateSHA1Hash(password);
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();

            if (userManager.UpdatePassword(gamertag, newPassword) == 1)
            {
                result = true;
            }

            return result;
        }

        private void BtnGoBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            this.NavigationService.GoBack();
        }

        private void ShowSuccessMessage()
        {
            MessageBox.Show(Properties.Resources.DlgSuccessfulChange, Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show(Properties.Resources.DlgWrongChange, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
