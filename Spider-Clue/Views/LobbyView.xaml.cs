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
        private string[] gamersInLobby;
        private bool isOwnerOfMatch = false;
        public LobbyView()
        {
            InitializeComponent();
            Utilities.PlayMainThemeSong(mainThemePlayer);
            MatchManager = new MatchManagerClient(new InstanceContext(this));
            LobbyManager = new LobbyManagerClient(new InstanceContext(this));
        }

        public void SetChatInLobby()
        {
            ChatView chatView = new ChatView();
            chatView.ConfigureWindow(MatchCode);
            chatFrame.NavigationService.Navigate(chatView);
        }

        public void SetMatchDataInPage(string matchCode)
        {
            MatchCode = matchCode;
            txtMatchCode.Text = matchCode;
            string gamertag = UserSingleton.Instance.GamerTag;

            MatchManager.ConnectToMatch(gamertag, matchCode);
            MatchManager.GetGamersInMatch(gamertag, matchCode);
            LobbyManager.ConnectToLobby(gamertag, matchCode);

            SetOwnerButtons();
            SetChatInLobby();
        }

        private Label FindCharacterLabel(string characterName)
        {
            Label resultLabel = null;

            switch (characterName)
            {
                case "PurpleCharacter":
                    resultLabel = lblPurpleCharacter;
                    break;
                case "WhiteCharacter":
                    resultLabel = lblWhiteCharacter;
                    break;
                case "RedCharacter":
                    resultLabel = lblRedCharacter;
                    break;
                case "GreenCharacter":
                    resultLabel = lblGreenCharacter;
                    break;
                case "YellowCharacter":
                    resultLabel = lblYellowCharacter;
                    break;
                case "BlueCharacter":
                    resultLabel = lblBlueCharacter;
                    break;
            }

            return resultLabel;
        }


        private void SetOwnerButtons()
        {
            isOwnerOfMatch = CheckMatchOwnership();
            if (isOwnerOfMatch)
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

        public void SetCharactersInLobby(string[] gamertags)
        {
            foreach (var gamertag in gamertags)
            {
                Character character = LobbyManager.GetCharacterPerGamer(gamertag);

                if(character != null)
                {
                    Label characterLabel = FindCharacterLabel(character.CharacterName);

                    if (characterLabel != null)
                    {
                        characterLabel.Content = gamertag;
                    }
                }
            }
        }

        private string GetIconImagePathForGamer(string gamertag)
        {
            IUserManager userManager = new SpiderClueService.UserManagerClient();
            string iconName = userManager.GetIcon(gamertag);

            return Utilities.GetImagePathForIcon(iconName);
        }

        public void ReceiveGamersInMatch(string[] gamertags)
        {
            Console.WriteLine("recibo gamers");
            gamersInLobby = gamertags;
            SetGamersList(gamertags);
            SetCharactersInLobby(gamertags);
        }

        public void KickPlayerFromMatch(string gamertag)
        {
            if (gamertag.Equals(UserSingleton.Instance.GamerTag))
            {
                GoToMainMenu();
            }
        }

        private void KickAllPlayersFromMatch()
        {
            string[] gamersToKick = GetGamersInLobbyExceptOwner();
            foreach (string gamer in gamersToKick)
            {
                LobbyManager.KickPlayerAsync(gamer);
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

            if(isOwnerOfMatch)
            {
                KickAllPlayersFromMatch();

            }

            MatchManager.LeaveMatchAsync(UserSingleton.Instance.GamerTag, MatchCode);
        }

        private void KickPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            string selectedGamer = OpenKickPlayerDialog();
            if (!selectedGamer.Equals(String.Empty))
            {
                LobbyManager.KickPlayer(selectedGamer);
            }
        }

        private string[] GetGamersInLobbyExceptOwner()
        {
            string gamertag = UserSingleton.Instance.GamerTag;
            string[] gamersToSend = gamersInLobby.Where(gamer => gamer != gamertag).ToArray();
            return gamersToSend;
        }

        private string OpenKickPlayerDialog()
        {
            KickPlayersView kickPlayersDialog = new KickPlayersView(GetGamersInLobbyExceptOwner());
            kickPlayersDialog.Owner = Window.GetWindow(this);
            kickPlayersDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            string selectedGamerToKick = string.Empty;

            if (kickPlayersDialog.ShowDialog() == true)
            {
                selectedGamerToKick = kickPlayersDialog.PlayerToKick;
            }

            return selectedGamerToKick;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();

        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LobbyManager.BeginMatch(MatchCode);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.DlgCommunicationException, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            }
            LobbyManager.Close();
            MatchManager.Close();
        }    
    }
}
