using Spider_Clue.Logic;
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
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
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
            int length = 8;
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            StringBuilder username = new StringBuilder();

            for(int i = 0; i < length;i++)
            {
                int index = random.Next(validChars.Length);
                username.Append(validChars[index]);
            }

            string randomUsername = username.ToString();
            string resourceName = Properties.Resources.GuessName;
            string finalUsername = $"{resourceName}{randomUsername}";

            return finalUsername;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            HandleLoginRequest();
        }

        private void HandleLoginRequest()
        {
            string username = txtUsername.Text;
            SecureString securePassword = txtPassword.SecurePassword;
            string password = new NetworkCredential(string.Empty, securePassword).Password;
        }
    }
}
