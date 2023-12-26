using Spider_Clue.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using Spider_Clue.SpiderClueService;

namespace Spider_Clue.Views
{
    public partial class AccountRecoveryView : Page
    {
        private readonly IUserManager userManager;

        public AccountRecoveryView()
        {
            InitializeComponent();
            userManager = new UserManagerClient();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            NavigationService.GoBack();
        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();

            string toEmail = txtEmailForRecovery.Text;

            if (ValidateEmail(toEmail) && CheckPlayerExistence(toEmail))
            {
                if (VerifyCode(toEmail))
                {
                    GoToChangePasswordView(toEmail);
                }
            }
            else
            {
                ShowErrorMessageBox(Properties.Resources.DlgInvalidData);
            }
        }

        private bool ValidateEmail( string toEmail)
        {
            return Validations.IsEmailValid(toEmail);
        }

        private bool VerifyCode(string toEmail)
        {
            return Utilities.SendEmailWithCode(toEmail, Window.GetWindow(this));
        }

        private void GoToChangePasswordView(string email)
        {
            Gamer gamer = userManager.GetGamerByEmail(email);
            PasswordRecoveryView changePasswordView = new PasswordRecoveryView();
            changePasswordView.SetGamertagInWindow(gamer.Gamertag);
            NavigationService.Navigate(changePasswordView);
        }

        private bool CheckPlayerExistence(string email)
        {
            return userManager.IsEmailExisting(email);
        }

        private void ShowErrorMessageBox(string errorMessage)
        {
            MessageBox.Show(errorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
