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
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            if (userManager.IsGamertagExisting(gamertag))
            {
                string icon = userManager.GetIcon(gamertag);
                SetGamerData(gamertag, icon);
                btnSendFriendRequest.Visibility = Visibility.Visible;
            } else
            {
                SetGamerData("No Results found", "NotFoundIcon.png");
            }
        }

        private void SetGamerData(string gamertag, string icon)
        {
            lblGamertag.Content = gamertag;
            string iconPath = Utilities.GetFriendImagePath(icon);
            this.DataContext = new { ImagePath = iconPath };
            
        }

        private void BtnSendFriendRequest_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
