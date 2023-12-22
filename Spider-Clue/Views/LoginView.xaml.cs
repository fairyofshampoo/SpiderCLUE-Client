using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            Utilities.PlayButtonClickSound();
            Register registerView = new Register();
            this.NavigationService.Navigate(registerView);
        }

        private void BtnGuestPlayer_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SetGuessPlayerData();
            DisplayMainMenuGuestView();
        }

        private void LblForgotPassword_Clicked(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            AccountRecoveryView accountRecoveryView = new AccountRecoveryView();
            this.NavigationService.Navigate(accountRecoveryView);
        }

        private void SetGuessPlayerData()
        {
            int minimumGamesWon = 0;
            string guestPlayerUsername = GenerateGuestPlayerUsername();
            UserSingleton.Instance.GamerTag = guestPlayerUsername;
            UserSingleton.Instance.GamesWon = minimumGamesWon;
            UserSingleton.Instance.ImageCode = "Icon0.jpg";
            UserSingleton.Instance.Name = "Guest";
            UserSingleton.Instance.LastName = "Player";
            UserSingleton.Instance.IsGuestPlayer = true;
        }

        private string GenerateGuestPlayerUsername()
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.RequestGuestPlayer();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            Utilities.PlayButtonClickSound();
            if (HandleLoginAttempt())
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
        }

        private void SaveSessionInSingleton()
        {
            string gamerTag = txtUsername.Text;
            Gamer gamer = GetGamerData(gamerTag);
            UserSingleton.Instance.Initialize(gamer);
        }

        private Gamer GetGamerData(String gamerTag)
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.GetGamerByGamertag(gamerTag);
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show(Properties.Resources.DlgWrongDataForLogin, Properties.Resources.DlgRegisterError, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool HandleLoginAttempt()
        {
            bool continueLogin = VerifyFields();
            if (continueLogin)
            {
                continueLogin = ValidateCredentials();
            }
            return continueLogin;
        }

        private bool VerifyFields()
        {
            String gamerTag = txtUsername.Text;
            string password = GetPassword();
            bool passwordValidation = VerifyPassword(password);
            bool gamerTagValidation = VerifyGamertag(gamerTag);

            if (!passwordValidation)
            {
                lblPasswordInvalid.Visibility = Visibility.Visible;
            }
            if (!gamerTagValidation)
            {
                lblGamertagInvalid.Visibility = Visibility.Visible;
            }

            return passwordValidation && gamerTagValidation;
        }

        private String GetPassword()
        {
            bool isChecked = btnPasswordVisibility.IsChecked ?? false;
            String password = txtPasswordDisplay.Text;

            if (!isChecked)
            {
                SecureString passwordToAccess = txtPassword.SecurePassword;
                password = new NetworkCredential(string.Empty, passwordToAccess).Password;
            }

            return password;
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

        private void DisplayMainMenuGuestView()
        {
            MainMenuForGuestView mainMenuGuestView = new MainMenuForGuestView();
            this.NavigationService.Navigate(mainMenuGuestView);
        }

        private bool ValidateCredentials()
        {
            string username = txtUsername.Text;
            string password = GetPassword();
            string passwordHashed = Utilities.CalculateSHA1Hash(password);
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.AuthenticateAccount(username, passwordHashed);
        }

        private void TypingGamerTag(object sender, TextChangedEventArgs e)
        {
            lblGamertagInvalid.Visibility = Visibility.Hidden;
        }

        private void TypingPassword(object sender, RoutedEventArgs e)
        {
            lblPasswordInvalid.Visibility = Visibility.Hidden;
        }

        private void BtnPasswordVisibility_Checked(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibility(true);
        }

        private void BtnPasswordVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibility(false);
        }

        private void TogglePasswordVisibility(bool isVisible)
        {
            if (isVisible)
            {
                bdrPassword.Visibility = Visibility.Collapsed;
                bdrPasswordDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                bdrPassword.Visibility = Visibility.Visible;
                bdrPasswordDisplay.Visibility = Visibility.Collapsed;
            }

            if (isVisible)
            {
                txtPasswordDisplay.Text = txtPassword.Password;
                SetPasswordIcon("InvisibleIcon.png");
            }
            else
            {
                txtPassword.Password = txtPasswordDisplay.Text;
                SetPasswordIcon("VisibleIcon.png");
            }
        }

        private void SetPasswordIcon(string iconPassword)
        {
            Uri newImageUri = new Uri($"/Images/Icons/{iconPassword}", UriKind.Relative);
            BitmapImage newImageSource = new BitmapImage(newImageUri);
            Image imgPasswordIcon = new Image();
            imgPasswordIcon = btnPasswordVisibility.Template.FindName("imgPasswordIcon", btnPasswordVisibility) as Image;
            imgPasswordIcon.Source = newImageSource;
        }

    }
}
