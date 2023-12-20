using Spider_Clue.SpiderClueService;
using System.ServiceModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Spider_Clue.Logic;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for LobbyView.xaml
    /// </summary>
    public partial class LobbyView : Page, IMatchManagerCallback, ILobbyManagerCallback
    {
        private string matchCode;
        public readonly MatchManagerClient MatchManager;
        public readonly LobbyManagerClient LobbyManager;
        public readonly IUserManager UserManager = new SpiderClueService.UserManagerClient();
        public LobbyView()
        {
            InitializeComponent();
            Utilities.PlayMainThemeSong(mainThemePlayer);
        }

        public void SetMatchDataInPage(string matchCode)
        {
            this.matchCode = matchCode;
            txtMatchCode.Text = matchCode;
            string gamertag = UserSingleton.Instance.GamerTag;

            MatchManager.ConnectToMatch(gamertag, matchCode);
            MatchManager.GetGamersInMatch(gamertag, matchCode);
        }

        private void SetGamersList(string[] gamertags)
        {
            List<GamerForListBox> gamersList = gamertags
                .Select(gamertag => new GamerForListBox
                {
                    ImageIconGamer = new BitmapImage(new Uri(GetIconImagePathForGamer(gamertag))),
                    GamerTag = gamertag
                })
                .ToList();

            GamersInMatchListBox.ItemsSource = gamersList;
        }

        private string GetIconImagePathForGamer(string gamertag)
        {
            string iconName = UserManager.GetIcon(gamertag);
            return Utilities.GetImagePathForIcon(iconName);
        }


        public void ReceiveGamersInMatch(string[] gamertags)
        {
            SetGamersList(gamertags);
        }

        public void KickPlayerFromMatch(string gamertag)
        {
            if (gamertag.Equals(UserSingleton.Instance.GamerTag))
            {
                GoToMainMenu();
            }
        }

        private void GoToMainMenu()
        {
            if (UserSingleton.Instance.IsGuestPlayer)
            {
                MainMenuForGuestView mainMenuForGuestView = new MainMenuForGuestView();
                this.NavigationService.Navigate(mainMenuForGuestView);
            }
            else
            {
                MainMenuView mainMenuView = new MainMenuView();
                this.NavigationService.Navigate(mainMenuView);
            }
        }

        private void KickPlayer_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SendInvitation_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void SendMailWithCodeMatch()
        {

        }

        public void StartGame()
        {
            try
            {
                LobbyManager.BeginMatch(matchCode);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.DlgCommunicationException, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                GoToMainMenu();
            }
        }
    }
}
