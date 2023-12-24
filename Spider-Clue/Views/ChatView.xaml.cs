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
    public partial class ChatView : Page, IChatManagerCallback
    {
        public readonly ChatManagerClient ChatManager;
        private string matchCode;
        public ChatView()
        {
            InitializeComponent();
            ChatManager = new ChatManagerClient(new InstanceContext(this));
        }

        public void ConfigureWindow(string matchCode)
        {
            this.matchCode = matchCode;
            string gamertag = UserSingleton.Instance.GamerTag;

            try
            {
                ChatManager.ConnectToChat(gamertag, matchCode);
            }
            catch (CommunicationException)
            {
                ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(txtMessage.Text);
            }
        }

        private void ShowErrorMessageBox(string errorMessage)
        {
            MessageBox.Show(errorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SendMessage(string messageText)
        {
            if (Validations.IsMessageValid(messageText))
            {
                var message = new Message
                {
                    Text = messageText,
                    GamerTag = UserSingleton.Instance.GamerTag,
                };

                try
                {
                    ChatManager.BroadcastMessage(matchCode, message);
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

        public void ReceiveMessages(Message[] messages)
        {
            listBoxChat.Items.Clear();
            foreach (Message message in messages)
            {
                string textToShow = string.Concat(message.GamerTag + ": " + message.Text + "\n");
                listBoxChat.Items.Add(textToShow);
                listBoxChat.ScrollIntoView(listBoxChat.Items[listBoxChat.Items.Count - 1]);
            }
        }

        public void CloseChat()
        {
            try
            {
                ChatManager.DisconnectFromChatAsync(UserSingleton.Instance.GamerTag);
            }
            catch (CommunicationException)
            {
                ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }

            ChatManager.Close();
        }
    }
}
