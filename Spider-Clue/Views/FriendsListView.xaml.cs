using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class FriendsListView : Page
    {
        public string[] FriendsConnected { get; set; }

        public FriendsListView(String[] friendsConnected)
        {
            InitializeComponent();
            FriendsConnected = friendsConnected;
            showFriendList();
        }

        private void showFriendList()
        {
            for(int index = 0; index< FriendsConnected.Length; index++)
            {
                Player player = new Player
                {
                    gamertag = FriendsConnected[index],
                    status = "Green"
                };
                FriendsConnectedGrid.Items.Add(player);
            }
        }

        public class Player
        {
            public string gamertag { get; set; }
            public string status { get; set; }
        }
        
    }
}
