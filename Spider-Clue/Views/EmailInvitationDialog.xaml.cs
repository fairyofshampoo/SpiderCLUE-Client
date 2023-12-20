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
        public readonly string MatchCode;
        public EmailInvitationDialog()
        {
            InitializeComponent();
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
