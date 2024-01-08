using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

namespace Spider_Clue.Views
{

    public partial class FriendsListView : Page, IFriendsManagerCallback
    {
        public string[] FriendsConnected { get; set; }

        public FriendsListView(String[] friendsConnected)
        {
            InitializeComponent();
            JoinFriendListView();
            FriendsConnected = friendsConnected;

            ShowFriendList();
        }

        private void JoinFriendListView()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IFriendsManager friendsManager = new SpiderClueService.FriendsManagerClient(new InstanceContext(this));
                friendsManager.JoinFriendsConnected(UserSingleton.Instance.GamerTag);
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

        private void ShowFriendList()
        {
            dtgFriendsConnected.Items.Clear();
            string[] friendList = GetFriends();
            string statusColor = "Red";

            if(friendList != null)
            {
                for (int firstIndex = 0; firstIndex < friendList.Length; firstIndex++)
                {
                    for (int secondIndex = 0; secondIndex < FriendsConnected.Length; secondIndex++)
                    {
                        if (friendList[firstIndex] == FriendsConnected[secondIndex])
                        {
                            statusColor = "Green";
                        }
                    }
                    Player player = new Player
                    {
                        Gamertag = friendList[firstIndex],
                        Status = statusColor
                    };
                    dtgFriendsConnected.Items.Add(player);
                    statusColor = "Red";
                }
            }           
        }

        private string [] GetFriends()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            string[] friendList = null;

            try
            {
                SpiderClueService.IFriendshipManager friendRequest = new SpiderClueService.FriendshipManagerClient();
                friendList = friendRequest.GetFriendList(UserSingleton.Instance.GamerTag);
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
            
            return friendList;
        }

        public class Player
        {
            public string Gamertag { get; set; }
            public string Status { get; set; }
        }

        private void BtnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            OpenDialogSendFriendRequest();
        }

        private void OpenDialogSendFriendRequest()
        {
            Window mainWindow = Window.GetWindow(this);
            SendFriendRequest sendFriendRequest = new SendFriendRequest();
            sendFriendRequest.Owner = mainWindow;
            sendFriendRequest.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            sendFriendRequest.ShowDialog();
        }

        private void BtnChangeFriendRequest(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            FriendsRequestView friendRequestView = new FriendsRequestView();
            NavigationService.Navigate(friendRequestView);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void BtnDeleteFiend_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if(ShowConfirmationMessage() == MessageBoxResult.OK)
            {
               string friend = GetFriendData(sender);
               DeleteFriend(friend);
               DeleteFriendsRequest(friend);
            }
        }

        private MessageBoxResult ShowConfirmationMessage()
        {
           return MessageBox.Show(Properties.Resources.DlgConfirmDeleteFriend, Properties.Resources.DeleteFriendTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private string GetFriendData (object sender)
        {
            string gamertag = "Not found";
            var button = sender as System.Windows.Controls.Button;
            if (button != null && button.DataContext is Player dataObject)
            {
               gamertag = dataObject.Gamertag;
            }
            return gamertag;
        }

        private void DeleteFriend(string friend)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IFriendshipManager friendship = new SpiderClueService.FriendshipManagerClient();
                friendship.DeleteFriend(UserSingleton.Instance.GamerTag, friend);
                ShowFriendList();
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

        private void DeleteFriendsRequest(string friend)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IFriendRequestManager friendRequest = new SpiderClueService.FriendRequestManagerClient();
                friendRequest.DeleteFriendRequest(UserSingleton.Instance.GamerTag, friend);
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

        public void ReceiveConnectedFriends(string[] connectedFriends)
        {
            FriendsConnected = connectedFriends;
            ShowFriendList();
        }
    }
}
