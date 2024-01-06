using Spider_Clue.SpiderClueService;
using System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Spider_Clue.Logic;

namespace Spider_Clue.Views
{
    public partial class LobbyView : Page, IMatchManagerCallback, ILobbyManagerCallback
    {
        private string MatchCode;
        public readonly MatchManagerClient MatchManager;
        public readonly LobbyManagerClient LobbyManager;
        private Dictionary<string, Pawn> gamersInLobby;
        private bool isOwnerOfMatch = false;
        private readonly ChatView chatView = new ChatView();

        public LobbyView()
        {
            InitializeComponent();
            Utilities.PlayMainThemeSong(meMainThemePlayer);
            MatchManager = new MatchManagerClient(new InstanceContext(this));
            LobbyManager = new LobbyManagerClient(new InstanceContext(this));
        }

        public void SetChatInLobby()
        {
            chatView.ConfigureWindow(MatchCode);
            frChat.NavigationService.Navigate(chatView);
        }

        public void SetMatchDataInPage(string matchCode)
        {
            MatchCode = matchCode;
            txtMatchCode.Text = matchCode;
            string gamertag = UserSingleton.Instance.GamerTag;

            MatchManager.ConnectToMatch(gamertag, matchCode);
            LobbyManager.ConnectToLobbyAsync(gamertag);

            SetOwnerButtons();
            SetChatInLobby();
        }

        private void SetOwnerButtons()
        {
            isOwnerOfMatch = CheckMatchOwnership();
            if (isOwnerOfMatch)
            {
                btnReady.Visibility = Visibility.Visible;
                brKickPlayer.Visibility = Visibility.Visible;
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

            lbxGamersInMatch.ItemsSource = gamersList;
        }

        private string[] GetArrayWithGamerTags(Dictionary<string, Pawn> gamers)
        {
            return gamers.Keys.ToArray();
        }

        private string GetIconImagePathForGamer(string gamertag)
        {
            IUserManager UserManager = new SpiderClueService.UserManagerClient();
            string iconName = UserManager.GetIcon(gamertag);
            return Utilities.GetImagePathForIcon(iconName);
        }

        public void ReceiveGamersInMatch(Dictionary<string, Pawn> characters)
        {
            gamersInLobby = characters;
            SetGamersList(GetArrayWithGamerTags(characters));
            SetCharactersInLobby(characters);
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

        public void GoToMainMenu()
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
            if (isOwnerOfMatch)
            {
                KickAllPlayersFromMatch();

            }
            chatView.CloseChat();
            MatchManager.LeaveMatchAsync(UserSingleton.Instance.GamerTag, MatchCode);
        }

        private void ClearCharacterLabels()
        {
            lblPurpleGamertag.Content = string.Empty;
            lblWhiteGamertag.Content = string.Empty;
            lblRedGamertag.Content = string.Empty;
            lblGreenGamertag.Content = string.Empty;
            lblYellowGamertag.Content = string.Empty;
            lblBlueGamertag.Content = string.Empty;
        }

        public void SetCharactersInLobby(Dictionary<string, Pawn> charactersInMatch)
        {
            ClearCharacterLabels();

            foreach (var gamer in charactersInMatch)
            {
                Pawn character = gamer.Value;
                if (character != null)
                {
                    Label characterLabel = FindCharacterLabel(character.Color);

                    if (characterLabel != null)
                    {
                        characterLabel.Content = gamer.Key;
                    }
                }
            }
        }

        private Label FindCharacterLabel(string characterName)
        {
            Label resultLabel = null;

            switch (characterName)
            {
                case "PurplePawn.png":
                    resultLabel = lblPurpleGamertag;
                    break;
                case "WhitePawn.png":
                    resultLabel = lblWhiteGamertag;
                    break;
                case "RedPawn.png":
                    resultLabel = lblRedGamertag;
                    break;
                case "GreenPawn.png":
                    resultLabel = lblGreenGamertag;
                    break;
                case "YellowPawn.png":
                    resultLabel = lblYellowGamertag;
                    break;
                case "BluePawn.png":
                    resultLabel = lblBlueGamertag;
                    break;
            }

            return resultLabel;
        }

        private void KickPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            string selectedGamer = OpenKickPlayerDialog();
            if (!string.IsNullOrEmpty(selectedGamer))
            {
                LobbyManager.KickPlayer(selectedGamer);
            }
        }

        private string[] GetGamersInLobbyExceptOwner()
        {
            string gamertag = UserSingleton.Instance.GamerTag;
            string[] gamersToSend = GetArrayWithGamerTags(gamersInLobby).Where(gamer => gamer != gamertag).ToArray();
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
            if (gamersInLobby.Count == 3)
            {
                try
                {
                    LobbyManager.BeginMatch(MatchCode);
                }
                catch (CommunicationException)
                {
                    MessageBox.Show(Properties.Resources.DlgCommunicationException, Properties.Resources.ErrorTitle);
                    GoToMainMenu();
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.DlgNotEnoughPlayers, Properties.Resources.InformationTitle);
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
            GameBoardView gameBoardView = new GameBoardView();
            gameBoardView.ConfigureWindow(MatchCode, gamersInLobby);
            NavigationService.Navigate(gameBoardView);
        }
    }
}