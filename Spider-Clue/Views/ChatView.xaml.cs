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

namespace Spider_Clue.Views
{
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

            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                ChatManager.ConnectToChat(gamertag, matchCode);
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

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(txtMessage.Text);
            }
        }

        private void SendMessage(string messageText)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

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
                DialogManager.ShowWarningMessageBox(Properties.Resources.DlgInvalidData);
            }

        }

        public void ReceiveMessages(Message[] messages)
        {
            lbxChat.Items.Clear();
            foreach (Message message in messages)
            {
                string textToShow = $"{message.GamerTag}: {message.Text}\n";
                lbxChat.Items.Add(textToShow);
                lbxChat.ScrollIntoView(lbxChat.Items[lbxChat.Items.Count - 1]);
            }
        }

        public void CloseChat()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                ChatManager.DisconnectFromChatAsync(UserSingleton.Instance.GamerTag);
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
    }
}