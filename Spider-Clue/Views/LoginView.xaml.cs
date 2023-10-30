using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
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
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LblRegister_Clicked(object sender, MouseButtonEventArgs mouseEvent)
        {
            Register registerView = new Register();
            this.NavigationService.Navigate(registerView);
        }

        private void BtnGuestPlayer_Click(object sender, RoutedEventArgs e)
        {
            SetGuessPlayerData();
            DisplayMainMenuView();
        }

        private void LblForgotPassword_Clicked(object sender, MouseButtonEventArgs e)
        {
            AccountRecoveryView accountRecoveryView = new AccountRecoveryView();
            this.NavigationService.Navigate(accountRecoveryView);
        }

        private void SetGuessPlayerData()
        {
            string guessPlayerUsername = GenerateGuessPlayerUsername();
            ///UserSingleton.Instance.Initialize(guessPlayerUsername,);
        }

        private string GenerateGuessPlayerUsername()
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();

            return userManager.RequestGuessPlayer();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (HandleLoginRequest())
            {
                SaveSession();
                DisplayMainMenuView();
            }
            else
            {
                ShowErrorMessage();
            }
        }
        private void SaveSession()
        {
            SaveSessionInSingleton();
            SaveSessionInServer();

        }
        private void SaveSessionInSingleton()
        {
            string gamerTag = txtUsername.Text;
            string name = txtUsername.Text;
            string lastName = txtUsername.Text;
            string email = txtUsername.Text;
            GetGamerData();
            UserSingleton.Instance.Initialize(gamerTag, name, lastName, email);
        }

        private void GetGamerData()
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            // mandar a llamar a metodo de get
        }
        private void ShowErrorMessage()
        {
            MessageBox.Show(Properties.Resources.DlgRegisterSuccessful, Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool HandleLoginRequest()
        {
            bool continueLogin = false;
            if (VerifyFields())
            {
                isBanned();
                continueLogin = ValidateCredentials();
            }
            return continueLogin;
        }

        private void isBanned()
        {

        }

        private bool VerifyFields()
        {
            String gamerTag = txtUsername.Text;
            SecureString passwordToAccess = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, passwordToAccess).Password;
            bool passwordValidation = VerifyPassword(password);
            bool gamerTagValidation = VerifyGamertag(gamerTag);

            if (!passwordValidation)
            {
                lblPasswordInvalid.Visibility = Visibility.Visible;
            }
            if (gamerTagValidation)
            {
                lblGamertagInvalid.Visibility = Visibility.Visible;
            }

            return passwordValidation && gamerTagValidation;
        }

        private bool VerifyPassword(string password)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(password))
            {
                isValid = false;
            }

            return isValid;
        }

        private bool VerifyGamertag(string gamerTag)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(gamerTag))
            {
                isValid = false;
            }

            return isValid;
        }

        private void DisplayMainMenuView()
        {
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void SaveSessionInServer()
        {
            //cuando tengamos el callback aquí se manda a llamar
        }

        private bool ValidateCredentials()
        {
            string username = txtUsername.Text;
            string passwordHashed = HashUtility.CalculateSHA1Hash(txtPassword.Password);
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.AuthenticateAccount(username, passwordHashed);
        }
    }
}
