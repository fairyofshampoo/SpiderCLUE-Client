using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : Page
    {
        public ChatView()
        {
            InitializeComponent();
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            SendMessage(txtMessage.Text);
        }
        private void ShowErrorMessageBox(string errorMessage)
        {
            MessageBox.Show(errorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SendMessage(string message)
        {
            if(Validations.IsMessageValid(message))
            {
                string senderGamertag = UserSingleton.Instance.GamerTag;

                try
                {
                    //MANDAR
                    txtMessage.Clear();
                }
                catch (CommunicationException)
                {
                    ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
                }
            }
            else
            {
                ShowErrorMessageBox(Properties.Resources.DlgInvalidData);
            }
            
        }
    }
}
