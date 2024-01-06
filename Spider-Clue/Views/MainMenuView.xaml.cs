using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Spider_Clue.Logic;
using System.Collections.Generic;
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
            ConnectToService();
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
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            Gamer gamer = userManager.GetGamerByGamertag(UserSingleton.Instance.GamerTag);
            UserSingleton.Instance.GamesWon = gamer.GamesWon;
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
            if (UserSingleton.Instance.GamerTag != null)
            {
                try
                {
                    SpiderClueService.ISessionManager sessionManager = new SpiderClueService.SessionManagerClient();
                    sessionManager.Disconnect(UserSingleton.Instance.GamerTag);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Disconnect: {ex.Message}");
                }
            }

            App.Current.Shutdown();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            string matchCode = CreateMatch();

            if (string.IsNullOrEmpty(matchCode))
            {
                MessageBox.Show("Error al crear la partida. Inténtalo de nuevo.", Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                GoToLobbyView(matchCode);
            }
        }

        private string CreateMatch()
        {
            SpiderClueService.IMatchCreationManager matchCreationManager = new SpiderClueService.MatchCreationManagerClient();
            return matchCreationManager.CreateMatch(UserSingleton.Instance.GamerTag);
        }

        private void GoToLobbyView(string matchCode)
        {
            LobbyView lobbyView = new LobbyView();
            lobbyView.SetMatchDataInPage(matchCode);
            this.NavigationService.Navigate(lobbyView);
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {
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
            friendsManagerClient.GetConnectedFriends(UserSingleton.Instance.GamerTag);
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

        private void ConnectToService()
        {
                int result = SessionManager.Connect(UserSingleton.Instance.GamerTag);
                if(result == 0)
                {
                    GoToLoginView();
                    MessageBox.Show("Ya has iniciado sesión", Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                } 
        }

        private void GoToLoginView()
        {
            LoginView loginView = new LoginView();
            UserSingleton.Instance.Clear();
            NavigationService.Navigate(loginView);
        }
    }
}
