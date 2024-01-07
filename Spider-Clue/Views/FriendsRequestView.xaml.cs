using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace Spider_Clue.Views
{
    public partial class FriendsRequestView : Page, IFriendsManagerCallback
    {
        private readonly FriendsManagerClient friendsManagerClient;
        public ObservableCollection<FriendRequest> FriendRequests { get; set; }

        public FriendsRequestView()
        {
            InitializeComponent();
            FriendRequests = new ObservableCollection<FriendRequest>();
            dtgFriendRequest.ItemsSource = FriendRequests;
            friendsManagerClient = new FriendsManagerClient(new InstanceContext(this));
            ShowFriendRequestList();          
        }

        private void ShowFriendRequestList()
        {
            string[] friendRequestList = GetFriendRequestList();
            if(friendRequestList != null)
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                for (int i = 0; i < friendRequestList.Length; i++)
                {
                    string gamerIcon = userManager.GetIcon(friendRequestList[i]);
                    string iconPath = Utilities.GetImagePathForIcon(gamerIcon);
                    FriendRequest friend = new FriendRequest
                    {
                        Gamertag = friendRequestList[i],
                        Icon = iconPath,
                    };
                    FriendRequests.Add(friend);
                }
            }
        }

        private string[] GetFriendRequestList()
        {
            SpiderClueService.IFriendRequestManager friendRequestManager = new SpiderClueService.FriendRequestManagerClient();
            string[] friendRequestList = friendRequestManager.GetFriendsRequest(UserSingleton.Instance.GamerTag);
            return friendRequestList;
        }

        private void BtnAcceptFriendRequest(object sender, RoutedEventArgs e)
        {
            GetFriendRequestData(sender, "Accepted");
        }

        private void BtnRejectFriendRequest(object sender, RoutedEventArgs e)
        {
            GetFriendRequestData(sender, "Reject");
        }

        private void GetFriendRequestData(object sender, string response)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null && button.DataContext is FriendRequest dataObject)
            {   
                string gamertag = dataObject.Gamertag;
                DeleteDataFromGrid(sender);
                ModifyFriendRequestStatus(gamertag, response);
            }
        }

        private void ModifyFriendRequestStatus(string gamertag, string response)
        {
            SpiderClueService.IFriendRequestManager friendRequestManager = new SpiderClueService.FriendRequestManagerClient();
            friendRequestManager.ResponseFriendRequest( UserSingleton.Instance.GamerTag,gamertag, response);
            if (response == "Accepted")
            {
                AddFriends(gamertag);
            }
        }

        private void DeleteDataFromGrid(object sender)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            FriendRequest request = (FriendRequest)btn.DataContext;
            FriendRequests.Remove(request);
        }

        private void AddFriends(string Friendgamertag)
        {
            SpiderClueService.IFriendshipManager friendshipManager = new SpiderClueService.FriendshipManagerClient();
            friendshipManager.AddFriend(UserSingleton.Instance.GamerTag, Friendgamertag);
        }

        public class FriendRequest
        {
            public string Gamertag { get; set; }
            public string Icon { get; set; }
        }

        private void BtnChangeFriendRequest(object sender, RoutedEventArgs e)
        {
            friendsManagerClient.GetConnectedFriends(UserSingleton.Instance.GamerTag);
        }

        public void ReceiveConnectedFriends(string[] connectedFriends)
        {
            FriendsListView friendListView = new FriendsListView(connectedFriends);
            NavigationService.Navigate(friendListView);
        }
    }
}
