using Spider_Clue.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using Spider_Clue.SpiderClueService;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for AccountRecoveryView.xaml
    /// </summary>
    public partial class AccountRecoveryView : Page
    {
        public AccountRecoveryView()
        {
            InitializeComponent();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            this.NavigationService.GoBack();
        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            String toEmail = txtEmailForRecovery.Text;

            if (CheckPlayerExistence(toEmail))
            {
                if (VerifyCode(toEmail))
                {
                    GoToChangePasswordView(toEmail);
                }
            }
            else
            {
                MessageBox.Show("Intente nuevamente, información inválida", Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool VerifyCode(string toEmail)
        {
            return Utilities.SendEmailWithCode(toEmail, Window.GetWindow(this));
        }

        private void GoToChangePasswordView(string email)
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            Gamer gamer = userManager.GetGamerByEmail(email);
            PasswordRecoveryView changePasswordView = new PasswordRecoveryView();
            changePasswordView.SetGamerInWindow(gamer);
            this.NavigationService.Navigate(changePasswordView);
        }

        private bool CheckPlayerExistence(string email)
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.IsAccountExisting(email);
        }
    }
}
