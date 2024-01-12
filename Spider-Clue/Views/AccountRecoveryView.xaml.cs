using Spider_Clue.Logic;
using System;
using System.Windows;
using System.ServiceModel;
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
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgInvalidData);
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
            LoggerManager logger = new LoggerManager(this.GetType());
            try
            {
                Gamer gamer = userManager.GetGamerByEmail(email);
                PasswordRecoveryView changePasswordView = new PasswordRecoveryView();
                changePasswordView.SetGamertagInWindow(gamer.Gamertag);
                NavigationService.Navigate(changePasswordView);
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

        private bool CheckPlayerExistence(string email)
        {
            bool result = false;
            LoggerManager logger = new LoggerManager(this.GetType());
            int resultExistence = Constants.SuccessfulOperation;
            try
            {
                resultExistence = userManager.IsEmailExisting(email);
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

            switch (resultExistence)
            {
                case Constants.SuccessfulOperation:
                    result = false;
                    break;
                case Constants.ExceptionResultOperation:
                    DialogManager.ShowErrorMessageBox(Properties.Resources.DlgDataBaseError);
                    result = false;
                    break;
                case Constants.DefaultResultOperation:
                    result = true;
                    break;
            }
            return result;
        }
    }
}
