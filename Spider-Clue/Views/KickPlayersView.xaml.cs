using Spider_Clue.Logic;
using System;
using System.ServiceModel;
using System.Windows;
using static Spider_Clue.Views.FriendsRequestView;

namespace Spider_Clue.Views
{
    public partial class KickPlayersView : Window 
    {
        public string[] PlayersInLobby { get; set; }
        public string PlayerToKick { get; set; }

        public KickPlayersView(string[] playersInLobby)
        {
            InitializeComponent();
            PlayersInLobby = playersInLobby;
            ShowPlayersInLobby();
        }

        public void ShowPlayersInLobby()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                for (int i = 0; i < PlayersInLobby.Length; i++)
                {
                    string gamerIcon = userManager.GetIcon(PlayersInLobby[i]);
                    string iconPath = Utilities.GetImagePathForIcon(gamerIcon);
                    FriendRequest player = new FriendRequest
                    {
                        Icon = iconPath,
                        Gamertag = PlayersInLobby[i],
                    };
                    dtgKickPlayers.Items.Add(player);
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

        private void BtnKickPlayer_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (ShowConfirmationMessageToKickPlayer() == MessageBoxResult.OK)
            {
                GetPlayerData(sender);
                DialogResult = true;
            }
        }

        private MessageBoxResult ShowConfirmationMessageToKickPlayer()
        {
            return MessageBox.Show(Properties.Resources.DlgKickPlayer, Properties.Resources.InformationTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public void GetPlayerData(object sender)
        {
            var button = sender as System.Windows.Controls.Button;

            if (button != null && button.DataContext is FriendRequest dataObject)
            {
                PlayerToKick = dataObject.Gamertag;
            }
        }
    }
}
