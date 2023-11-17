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
                    gamertag = friendList[firstIndex],
                    status = statusColor
                };
                FriendsConnectedGrid.Items.Add(player);
                statusColor = "Red";
            }           
        }

        private string [] GetFriends()
        {
            SpiderClueService.IFriendRequestManager friendRequest = new SpiderClueService.FriendRequestManagerClient();
            string [] friendList = friendRequest.GetFriendList(UserSingleton.Instance.GamerTag);
            return friendList;
        }

        public class Player
        {
            public string gamertag { get; set; }
            public string status { get; set; }
        }
        
    }
}
