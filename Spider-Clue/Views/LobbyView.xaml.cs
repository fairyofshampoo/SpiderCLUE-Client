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
        private string matchCode;
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
            chatView.ConfigureWindow(matchCode);
            frChat.NavigationService.Navigate(chatView);
        }

        public void SetMatchDataInPage(string matchCode)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                this.matchCode = matchCode;
                txtMatchCode.Text = matchCode;
                string gamertag = UserSingleton.Instance.GamerTag;

                MatchManager.ConnectToMatch(gamertag, matchCode);
                LobbyManager.ConnectToLobbyAsync(gamertag);

                SetOwnerButtons();
                SetChatInLobby();
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
            
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
            bool result = false;
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                string gamertag = UserSingleton.Instance.GamerTag;
                result = LobbyManager.IsOwnerOfTheMatch(gamertag, matchCode);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }

            return result;
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
            LoggerManager logger = new LoggerManager(this.GetType());
            string iconName = "Icon0.png";
            try
            {
                IUserManager UserManager = new SpiderClueService.UserManagerClient();
                iconName = UserManager.GetIcon(gamertag);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }

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
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                string[] gamersToKick = GetGamersInLobbyExceptOwner();
                foreach (string gamer in gamersToKick)
                {
                    LobbyManager.KickPlayerAsync(gamer);
                }
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
            
        }

        public void GoToMainMenu()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
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
                MatchManager.LeaveMatchAsync(UserSingleton.Instance.GamerTag, matchCode);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
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

        private void BrKickPlayer_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                string selectedGamer = OpenKickPlayerDialog();
                if (!string.IsNullOrEmpty(selectedGamer))
                {
                    LobbyManager.KickPlayer(selectedGamer);
                }
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
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
            Utilities.PlayButtonClickSound();
            GoToMainMenu();
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (gamersInLobby.Count == Constants.LimitOfGamersInMatch)
            {
                LoggerManager logger = new LoggerManager(this.GetType());

                try
                {
                    LobbyManager.BeginMatch(matchCode);
                }
                catch (EndpointNotFoundException endpointException)
                {
                    logger.LogError(endpointException);
                    DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
                }
                catch (TimeoutException timeoutException)
                {
                    logger.LogError(timeoutException);
                    DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
                }
                catch (CommunicationException communicationException)
                {
                    logger.LogError(communicationException);
                    DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
                }
                catch (Exception exception)
                {
                    logger.LogFatal(exception);
                    DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.DlgNotEnoughPlayers, Properties.Resources.InformationTitle);
            }
        }

        private void BrSendInvitation_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SendMailWithCodeMatch();
        }

        private void SendMailWithCodeMatch()
        {
            EmailInvitationDialog invitationDialog = new EmailInvitationDialog();
            invitationDialog.Owner = Window.GetWindow(this);
            invitationDialog.SetMatchCodeInPage(matchCode);
            invitationDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            invitationDialog.ShowDialog();
        }

        public void StartGame()
        {
            GameBoardView gameBoardView = new GameBoardView();
            gameBoardView.ConfigureWindow(matchCode, gamersInLobby);
            NavigationService.Navigate(gameBoardView);
        }
    }
}