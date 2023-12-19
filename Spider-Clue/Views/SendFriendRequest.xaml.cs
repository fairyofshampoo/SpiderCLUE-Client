using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for SendFriendRequest.xaml
    /// </summary>
    public partial class SendFriendRequest : Window
    {
        public SendFriendRequest()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, MouseButtonEventArgs e)
        {
            searchData.Visibility = Visibility.Visible;
            string gamertag = txtSearchGamer.Text;
            IUserManager userManager = new SpiderClueService.UserManagerClient();
            if (userManager.IsGamertagExisting(gamertag))
            {
                if (IsSearchValid(gamertag))
                {
                    string icon = userManager.GetIcon(gamertag);
                    SetGamerData(gamertag, icon);
                    btnSendFriendRequest.Visibility = Visibility.Visible;
                }else
                {
                    string icon = userManager.GetIcon(gamertag);
                    SetGamerData(gamertag, icon);
                }
            }
            else
            {
                ShowNotFoundMessage();
            }
        }

        private Boolean IsSearchValid(string friendGamertag)
        {
            return AreNotFriends(friendGamertag) && IsNotASelfFriendRequest(friendGamertag) && ThereIsNoFriendRequest(friendGamertag);
        }

        private Boolean AreNotFriends(string friendGamertag)
        {
            SpiderClueService.IFriendshipManager friendshipManager = new SpiderClueService.FriendshipManagerClient();
            return friendshipManager.AreNotFriends(UserSingleton.Instance.GamerTag, friendGamertag);
        }

        private Boolean ThereIsNoFriendRequest(string friendGamertag)
        {
            SpiderClueService.IFriendshipManager friendshipManager = new SpiderClueService.FriendshipManagerClient();
            return friendshipManager.ThereIsNoFriendRequest(UserSingleton.Instance.GamerTag, friendGamertag);
        }

        private Boolean IsNotASelfFriendRequest(string friendGamertag)
        {
            Boolean result = false;
            if (friendGamertag != UserSingleton.Instance.GamerTag)
            {
                result = true;
            }
            return result;
        }

        private void ShowNotFoundMessage()
        {
            btnSendFriendRequest.Visibility = Visibility.Collapsed;
            lblGamertag.Content = Properties.Resources.ResultsNotFoundMessage;
            string iconPath = Utilities.GetImagePathForImages() + "Icons\\NotFoundIcon.png";
            this.DataContext = new { ImagePath = iconPath };
        }

        private void SetGamerData(string gamertag, string icon)
        {
            lblGamertag.Content = gamertag;
            string iconPath = Utilities.GetImagePathForImages() + "Avatars\\" + icon;
            this.DataContext = new { ImagePath = iconPath };
        }

        private void BtnSendFriendRequest_Click(object sender, RoutedEventArgs e)
        {
            IFriendRequestManager friend = new SpiderClueService.FriendRequestManagerClient();
            friend.CreateFriendRequest(UserSingleton.Instance.GamerTag, lblGamertag.Content.ToString());
            this.Close();
        }

    }
}