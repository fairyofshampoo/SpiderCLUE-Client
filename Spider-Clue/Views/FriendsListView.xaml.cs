using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
using static Spider_Clue.Views.FriendsRequestView;

namespace Spider_Clue.Views
{

    public partial class FriendsListView : Page
    {
        public string[] FriendsConnected { get; set; }

        public FriendsListView(String[] friendsConnected)
        {
            InitializeComponent();
            FriendsConnected = friendsConnected;
            ShowFriendList();
        }

        private void ShowFriendList()
        {
            string[] friendList = GetFriends();
            string statusColor = "Red";
            for(int firstIndex = 0; firstIndex < friendList.Length; firstIndex++)
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
                FriendsConnectedGrid.Items.Add(player);
                statusColor = "Red";
            }           
        }

        private string [] GetFriends()
        {
            SpiderClueService.IFriendshipManager friendRequest = new SpiderClueService.FriendshipManagerClient();
            string [] friendList = friendRequest.GetFriendList(UserSingleton.Instance.GamerTag);
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
            Console.WriteLine(gamertag);
            return gamertag;
        }

        private void DeleteFriend(string friend)
        {
            SpiderClueService.IFriendshipManager friendship = new SpiderClueService.FriendshipManagerClient();
            friendship.DeleteFriend(UserSingleton.Instance.GamerTag, friend);
        }

        private void DeleteFriendsRequest(string friend)
        {
            SpiderClueService.IFriendRequestManager friendRequest = new SpiderClueService.FriendRequestManagerClient();
            friendRequest.DeleteFriendRequest(UserSingleton.Instance.GamerTag, friend);
        }
    }
}
