using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static Spider_Clue.Views.FriendsListView;
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
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            for (int Index = 0; Index < PlayersInLobby.Length; Index++)
            {
                string gamerIcon = userManager.GetIcon(PlayersInLobby[Index]);
                string iconPath = Utilities.GetImagePathForIcon(gamerIcon);
                FriendRequest player = new FriendRequest
                {
                    Icon = iconPath,
                    Gamertag = PlayersInLobby[Index],
                };
                dtgKickPlayers.Items.Add(player);
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
            return MessageBox.Show(Properties.Resources.DlgConfirmDeleteFriend, Properties.Resources.DeleteFriendTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
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
