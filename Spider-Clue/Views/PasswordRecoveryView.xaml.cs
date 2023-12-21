using Spider_Clue.SpiderClueService;
using System.Net;
using System.Security;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.Windows;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for PasswordRecoveryView.xaml
    /// </summary>
    public partial class PasswordRecoveryView : Page
    {
        private Gamer gamer;
        public PasswordRecoveryView()
        {
            InitializeComponent();
        }

        public void SetGamerInWindow(Gamer gamer)
        {
            this.gamer = gamer;
        }

        private bool ValidatePassword()
        {
            
            bool passwordValid = IsPasswordValid();
            bool passwordsMatching = ArePasswordsMatching();

            return passwordValid && passwordsMatching;
        }

        private bool IsPasswordValid()
        {
            SecureString securePassword = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
            bool passwordValid = Validations.IsPasswordValid(password);

            if(!passwordValid)
            {
                lblPasswordInvalid.Visibility = Visibility.Visible;
            }

            return passwordValid;
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
            gamer.Password = Utilities.CalculateSHA1Hash(password);
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();

            if (userManager.UpdateGamerTransaction(gamer) == 1)
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
