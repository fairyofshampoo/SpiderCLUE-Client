using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Spider_Clue.Views
{
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LblRegister_Click(object sender, MouseButtonEventArgs mouseEvent)
        {
            Utilities.PlayButtonClickSound();
            Register registerView = new Register();
            this.NavigationService.Navigate(registerView);
        }

        private void BtnGuestPlayer_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SetGuessPlayerData();
        }

        private void LblForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            AccountRecoveryView accountRecoveryView = new AccountRecoveryView();
            this.NavigationService.Navigate(accountRecoveryView);
        }

        private void SetGuessPlayerData()
        {
            int minimumGamesWon = 0;
            string defaultImageIcon = "Icon0.jpg";
            string defaultName = "Guest";
            string defaultLastName = "Player";
            string guestPlayerUsername = GenerateGuestPlayerUsername();

            if(!string.IsNullOrEmpty(guestPlayerUsername))
            {
                UserSingleton.Instance.GamerTag = guestPlayerUsername;
                UserSingleton.Instance.GamesWon = minimumGamesWon;
                UserSingleton.Instance.ImageCode = defaultImageIcon;
                UserSingleton.Instance.Name = defaultName;
                UserSingleton.Instance.LastName = defaultLastName;
                UserSingleton.Instance.IsGuestPlayer = true;
                DisplayMainMenuGuestView();
            }
        }

        private string GenerateGuestPlayerUsername()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            string username = string.Empty;
            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                username = userManager.RequestGuestPlayer();
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
            
            return username;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (HandleLoginAttempt())
            {
                LoggerManager logger = new LoggerManager(this.GetType());

                try
                {
                    ISessionManager sessionManager = new SpiderClueService.SessionManagerClient();

                    if (sessionManager.IsGamerAlreadyOnline(txtUsername.Text))
                    {
                        ShowUserAlreadyOnlineMessage();
                    }
                    else
                    {
                        SaveSession();
                        DisplayMainMenuView();
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
            }
            else
            {
                ShowWrongDataMessage();
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
            if(gamer != null)
            {
                UserSingleton.Instance.Initialize(gamer);
            }
        }

        private Gamer GetGamerData(string gamerTag)
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            Gamer gamer = null;

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                gamer = userManager.GetGamerByGamertag(gamerTag);
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
            
            return gamer;
        }

        private void ShowWrongDataMessage()
        {
            MessageBox.Show(Properties.Resources.DlgWrongDataForLogin, Properties.Resources.DlgRegisterError, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowUserAlreadyOnlineMessage()
        {
            DialogManager.ShowWarningMessageBox(Properties.Resources.DlgAlreadyLogin);
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
            string gamerTag = txtUsername.Text;
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

        private string GetPassword()
        {
            bool isChecked = tgbtnPasswordVisibility.IsChecked ?? false;
            string password = txtPasswordDisplay.Text;

            if (!isChecked)
            {
                SecureString passwordToAccess = pwbPassword.SecurePassword;
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
            int successfulResult = 1;
            MainMenuView mainMenuView = new MainMenuView();

            if(mainMenuView.ConnectToService() == successfulResult)
            {
                this.NavigationService.Navigate(mainMenuView);
            }
        }

        private void DisplayMainMenuGuestView()
        {
            MainMenuForGuestView mainMenuGuestView = new MainMenuForGuestView();
            this.NavigationService.Navigate(mainMenuGuestView);
        }

        private bool ValidateCredentials()
        {
            bool result = false;
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                string username = txtUsername.Text;
                string password = GetPassword();
                string passwordHashed = Utilities.CalculateSHA1Hash(password);
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                result = userManager.AuthenticateAccount(username, passwordHashed);
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

        private void TypingGamerTag(object sender, TextChangedEventArgs e)
        {
            lblGamertagInvalid.Visibility = Visibility.Hidden;
        }

        private void TypingPassword(object sender, RoutedEventArgs e)
        {
            lblPasswordInvalid.Visibility = Visibility.Hidden;
        }

        private void TgbtnPasswordVisibility_Checked(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibility(true);
        }

        private void TgbtnPasswordVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibility(false);
        }

        private void TogglePasswordVisibility(bool isVisible)
        {
            if (isVisible)
            {
                brPassword.Visibility = Visibility.Collapsed;
                brPasswordDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                brPassword.Visibility = Visibility.Visible;
                brPasswordDisplay.Visibility = Visibility.Collapsed;
            }

            if (isVisible)
            {
                txtPasswordDisplay.Text = pwbPassword.Password;
                SetPasswordIcon("InvisibleIcon.png");
            }
            else
            {
                pwbPassword.Password = txtPasswordDisplay.Text;
                SetPasswordIcon("VisibleIcon.png");
            }
        }

        private void SetPasswordIcon(string iconPassword)
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            try
            {
                Image imgPasswordIcon = tgbtnPasswordVisibility.Template.FindName("imgPasswordIcon", tgbtnPasswordVisibility) as Image;

                if (imgPasswordIcon != null)
                {
                    imgPasswordIcon.Source = new BitmapImage(new Uri($"/Images/Icons/{iconPassword}", UriKind.Relative));
                }
            }
            catch (UriFormatException uriException)
            {
                logger.LogError(uriException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgIconException);
            }
        }
    }
}