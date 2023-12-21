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
    public partial class LobbyView : Page, IMatchManagerCallback, ILobbyManagerCallback
    {
        private string MatchCode;
        public readonly MatchManagerClient MatchManager;
        public readonly LobbyManagerClient LobbyManager;
        public readonly IUserManager UserManager = new SpiderClueService.UserManagerClient();
        public LobbyView()
        {
            InitializeComponent();
            Utilities.PlayMainThemeSong(mainThemePlayer);
            MatchManager = new MatchManagerClient(new InstanceContext(this));
            LobbyManager = new LobbyManagerClient(new InstanceContext(this));

        }

        public void SetMatchDataInPage(string matchCode)
        {
            MatchCode = matchCode;
            txtMatchCode.Text = matchCode;
            string gamertag = UserSingleton.Instance.GamerTag;

            MatchManager.ConnectToMatch(gamertag, matchCode);
            MatchManager.GetGamersInMatch(gamertag, matchCode);

            SetOwnerButtons();
        }

        private void SetOwnerButtons()
        {
            if (CheckMatchOwnership())
            {
                btnReady.Visibility = Visibility.Visible;
                bdrKickPlayer.Visibility = Visibility.Visible;
            }
        }

        private bool CheckMatchOwnership()
        {
            string gamertag = UserSingleton.Instance.GamerTag;
            return LobbyManager.IsOwnerOfTheMatch(gamertag, MatchCode);
        }
        private void SetGamersList(string[] gamertags)
        {
            List<GamerForListBox> gamersList = gamertags
                .Select(gamertag => new GamerForListBox
                {
                    ImageIconGamer = new BitmapImage(new Uri(GetIconImagePathForGamer(gamertag))),
                    GamerName = gamertag
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
            //OpenKickPlayerDialog(MatchCode);
        }

        /*private string[] OpenKickPlayerDialog(string matchCode)
        {

        }*/


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SendInvitation_Click(object sender, MouseButtonEventArgs e)
        {
            SendMailWithCodeMatch();
        }

        private void SendMailWithCodeMatch()
        {
            EmailInvitationDialog invitationDialog = new EmailInvitationDialog();
            invitationDialog.Owner = Window.GetWindow(this);
            invitationDialog.SetMatchCodeInPage(MatchCode);
            invitationDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            invitationDialog.ShowDialog();
        }

        public void StartGame()
        {
            try
            {
                LobbyManager.BeginMatch(MatchCode);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.DlgCommunicationException, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                GoToMainMenu();
            }
        }
    }
}
