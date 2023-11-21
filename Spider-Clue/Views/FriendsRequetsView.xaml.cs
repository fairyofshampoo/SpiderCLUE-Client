using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Spider_Clue.Views.FriendsRequetsView;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for FriendsRequetsView.xaml
    /// </summary>
    public partial class FriendsRequetsView : Page
    {

        public ObservableCollection<FriendRequest> FriendRequests { get; set; }

        public FriendsRequetsView()
        {
            InitializeComponent();
            FriendRequests = new ObservableCollection<FriendRequest>();
            FriendRequestGrid.ItemsSource = FriendRequests;
            ShowFriendRequestList();          
        }

        private void ShowFriendRequestList()
        {
            string[] friendRequestList = GetFriendRequestList();
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            for (int i = 0; i < friendRequestList.Length; i++)
            {
                string gamerIcon = userManager.GetIcon(friendRequestList[i]);
                string iconPath = Utilities.GetFriendImagePath(gamerIcon);
                FriendRequest friend = new FriendRequest
                {
                    gamertag = friendRequestList[i],
                    icon = iconPath,
                };
                FriendRequests.Add(friend);
            }
        }

        private string[] GetFriendRequestList()
        {
            SpiderClueService.IFriendRequestManager friendRequestManager = new SpiderClueService.FriendRequestManagerClient();
            string[] friendRequestList = friendRequestManager.GetFriendsRequets(UserSingleton.Instance.GamerTag);
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
                string gamertag = dataObject.gamertag;
                DeleteDataFromGrid(gamertag, sender);
                modifyFriendRequestStatus(gamertag, response);
            }
        }

        private void modifyFriendRequestStatus(string gamertag, string response)
        {
            SpiderClueService.IFriendRequestManager friendRequestManager = new SpiderClueService.FriendRequestManagerClient();
            friendRequestManager.ResponseFriendRequest( UserSingleton.Instance.GamerTag,gamertag, response);
            if (response == "Accepted")
            {
                AddFriends(gamertag);
            }
        }

        private void DeleteDataFromGrid(string gamertag, object sender)
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
            public string gamertag { get; set; }
            public string icon { get; set; }
        }
    }
}
