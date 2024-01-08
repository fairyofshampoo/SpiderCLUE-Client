using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System.ServiceModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace Spider_Clue.Views
{

    public partial class EmailInvitationDialog : Window
    {
        private string MatchCode;
        public readonly IInvitationManager InvitationManager = new SpiderClueService.InvitationManagerClient();
        public EmailInvitationDialog()
        {
            InitializeComponent();
        }

        public void SetMatchCodeInPage(string matchCode)
        {
            MatchCode = matchCode;
            lblCodeMatch.Content = matchCode;
        }
        private void Copy_Click(object sender, MouseButtonEventArgs e)
        {
            CopyMatchCode();
        }
        private void CopyMatchCode()
        {
            Clipboard.SetText(MatchCode);
            DialogManager.ShowSuccessMessageBox(Properties.Resources.DlgMatchCodeCopied);
        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateEmail(txtEmail.Text))
            {
                SendInvitation();
            }
            else
            {
                DialogManager.ShowWarningMessageBox(Properties.Resources.DlgInvalidData);
            }

        }

        private void SendInvitation()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                bool invitationResult = InvitationManager.SendInvitation(txtEmail.Text, MatchCode, UserSingleton.Instance.GamerTag);

                if (invitationResult)
                {
                    DialogManager.ShowSuccessMessageBox(Properties.Resources.DlgInvitationSent);
                }
                else
                {
                    DialogManager.ShowErrorMessageBox(Properties.Resources.DlgErrorInvitation);
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
        private bool ValidateEmail(string toEmail)
        {
            return Validations.IsEmailValid(toEmail);
        }

        private void SendEmail_Click(object sender, MouseButtonEventArgs e)
        {
            stpCopyCode.Visibility = Visibility.Collapsed;
            stpSendEmail.Visibility = Visibility.Visible;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            stpCopyCode.Visibility = Visibility.Visible;
            stpSendEmail.Visibility = Visibility.Collapsed;
        }
    }
}
