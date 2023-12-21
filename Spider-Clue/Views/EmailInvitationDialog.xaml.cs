using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for EmailInvitationDialog.xaml
    /// </summary>
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
            MessageBox.Show(Properties.Resources.DlgMatchCodeCopied, Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateEmail(txtEmail.Text))
            {
                SendInvitation();
            }
            else
            {
                ShowErrorMessageBox(Properties.Resources.DlgInvalidData);
            }

        }
        private void ShowErrorMessageBox(string errorMessage)
        {
            MessageBox.Show(errorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccessMessageBox(string successMessage)
        {
            MessageBox.Show(successMessage, Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SendInvitation()
        {
            bool invitationResult = InvitationManager.SendInvitation(txtEmail.Text, MatchCode, UserSingleton.Instance.GamerTag);
            if (invitationResult)
            {
                ShowSuccessMessageBox(Properties.Resources.DlgInvitationSent);
            }
            else
            {
                ShowErrorMessageBox(Properties.Resources.DlgErrorInvitation);
            }
        }
        private bool ValidateEmail(string toEmail)
        {
            return Validations.IsEmailValid(toEmail);
        }

        private void SendEmail_Click(object sender, MouseButtonEventArgs e)
        {
            stckPanelCopyCode.Visibility = Visibility.Collapsed;
            stckPanelSendEmail.Visibility = Visibility.Visible;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            stckPanelCopyCode.Visibility = Visibility.Visible;
            stckPanelSendEmail.Visibility = Visibility.Collapsed;
        }
    }
}
