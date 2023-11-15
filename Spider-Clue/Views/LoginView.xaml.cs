using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Utilities.PlayButtonClickSound();
            Register registerView = new Register();
            this.NavigationService.Navigate(registerView);
        }

        private void BtnGuestPlayer_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SetGuessPlayerData();
            DisplayMainMenuView();
        }

        private void LblForgotPassword_Clicked(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
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
            Console.WriteLine("aqui se saca el codigo para meterlo al singleton" + gamer.ImageCode);
            UserSingleton.Instance.Initialize(gamer);
        }

        private Gamer GetGamerData(String gamerTag)
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.GetGamer(gamerTag);
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show("Verifique el correo y contraseña, sean correctos. No se ha podido iniciar sesión", Properties.Resources.DlgRegisterError, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool HandleLoginAttempt()
        {
            bool continueLogin = VerifyFields();
            if (continueLogin)
            {
                continueLogin = ValidateCredentials();
                if (continueLogin)
                {
                    continueLogin = !IsBanned();
                }
            }
            return continueLogin;
        }

        private bool IsBanned()
        {
            bool banned = false;
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            if (userManager.GetBannedStatus(txtUsername.Text) == 1)
            {
                banned = true;
                ShowBannedDialog();
            }

            return banned;
        }

        private void ShowBannedDialog()
        {
            MessageBox.Show("Su cuenta ha sido baneada", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void SetPasswordIcon(string iconFileName)
        {
            Uri newImageUri = new Uri($"/Images/{iconFileName}", UriKind.Relative);
            BitmapImage newImageSource = new BitmapImage(newImageUri);
            Image imgPasswordIcon = btnPasswordVisibility.Template.FindName("imgPasswordIcon", btnPasswordVisibility) as Image;
            imgPasswordIcon.Source = newImageSource;
        }

    }
}
