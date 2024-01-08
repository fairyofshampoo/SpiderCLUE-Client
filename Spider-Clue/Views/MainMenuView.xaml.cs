using System;
using System.Windows;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.ServiceModel;
using Spider_Clue.SpiderClueService;

namespace Spider_Clue.Views
{
    public partial class MainMenuView : Page, IFriendsManagerCallback
    {
        private readonly FriendsManagerClient friendsManagerClient;
        private string[] connectedFriends;
        public readonly ISessionManager SessionManager = new SpiderClueService.SessionManagerClient();
        public MainMenuView()
        {
            InitializeComponent();
            Loaded += PageLoaded;
            Utilities.PlayMainThemeSong(mainThemePlayer);
            friendsManagerClient = new FriendsManagerClient(new InstanceContext(this));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            SetGamerData();
        }
        private void SetGamerData()
        {
            lblUserName.Content = UserSingleton.Instance.GamerTag;
            UpdateGamesWon();
            lblLevel.Content = UserSingleton.Instance.GamesWon;
            string iconPath = Utilities.GetImagePathForIcon(UserSingleton.Instance.ImageCode);
            this.DataContext = new { ImagePath = iconPath };
        }

        private void UpdateGamesWon()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                Gamer gamer = userManager.GetGamerByGamertag(UserSingleton.Instance.GamerTag);
                UserSingleton.Instance.GamesWon = gamer.GamesWon;
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
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SettingsView settingsView = new SettingsView();
            this.NavigationService.Navigate(settingsView);
        }
        private void BtnJoinToParty_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            OpenDialogForSearchGame();
        }

        private void OpenDialogForSearchGame()
        {
            Window mainWindow = Window.GetWindow(this);
            SearchGameView searchGamePopUp = new SearchGameView();
            searchGamePopUp.Owner = mainWindow;
            searchGamePopUp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SearchGameView.MenuView = this;
            searchGamePopUp.ShowDialog();
        }

        private void BtnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            PersonalInformationView personInformation = new PersonalInformationView();
            NavigationService.Navigate(personInformation);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (UserSingleton.Instance.GamerTag != null)
            {
                LoggerManager logger = new LoggerManager(this.GetType());

                try
                {
                    SpiderClueService.ISessionManager sessionManager = new SpiderClueService.SessionManagerClient();
                    sessionManager.Disconnect(UserSingleton.Instance.GamerTag);
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

            App.Current.Shutdown();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();

            string matchCode = CreateMatch();

            if (string.IsNullOrEmpty(matchCode))
            {
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgMatchCreationError);
            }
            else
            {
                GoToLobbyView(matchCode);
            }
        }

        private string CreateMatch()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            string matchCode = string.Empty;

            try
            {
                SpiderClueService.IMatchCreationManager matchCreationManager = new SpiderClueService.MatchCreationManagerClient();
                matchCode = matchCreationManager.CreateMatch(UserSingleton.Instance.GamerTag);
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
            
            return matchCode;
        }

        private void GoToLobbyView(string matchCode)
        {
            LobbyView lobbyView = new LobbyView();
            lobbyView.SetMatchDataInPage(matchCode);
            this.NavigationService.Navigate(lobbyView);
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            ShowFriendsList();
        }
        private void BtnTopGlobal_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            Top3GlobalView top3GlobalView = new Top3GlobalView();
            NavigationService.Navigate(top3GlobalView);
        }

        private void ShowFriendsList()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                friendsManagerClient.GetConnectedFriends(UserSingleton.Instance.GamerTag);
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

        public void ReceiveConnectedFriends(string[] connectedFriends)
        {
            this.connectedFriends = connectedFriends;
            GoToFriendsListView();
        }

        private void GoToFriendsListView()
        {
            FriendsListView friendListView = new FriendsListView(connectedFriends);
            NavigationService.Navigate(friendListView);
        }

        public int ConnectToService()
        {
            int result = -1;
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                result = SessionManager.Connect(UserSingleton.Instance.GamerTag);
                if (result == -1)
                {
                    DialogManager.ShowWarningMessageBox(Properties.Resources.DlgAlreadyLogin);
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

            return result;
        }
    }
}
