using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Spider_Clue.Logic;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ServiceModel;
using Spider_Clue.SpiderClueService;

namespace Spider_Clue.Views
{
    public partial class MainMenuView : Page, IFriendsManagerCallback
    {
        private readonly FriendsManagerClient friendsManagerClient;
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
            lblLevel.Content = UserSingleton.Instance.Level;
            string iconPath = Utilities.GetImagePathForIcon(UserSingleton.Instance.ImageCode);
            this.DataContext = new { ImagePath = iconPath };
        }
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SettingsView settingsView = new SettingsView();
            NavigationService.Navigate(settingsView);
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
            App.Current.Shutdown();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            CreateMatch();
       //     GoToLobbyView();
        }

        private void CreateMatch()
        {
            SpiderClueService.IMatchCreationManager matchCreationManager = new SpiderClueService.MatchCreationManagerClient();
            matchCreationManager.CreateMatch(UserSingleton.Instance.GamerTag);
        }

        private void GoToLobbyView()
        {
            
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {
            ShowFriendsList();
        }

        private void ShowFriendsList()
        {
            friendsManagerClient.GetConnectedFriends(UserSingleton.Instance.GamerTag);
        }

        public void ReceiveConnectedFriends(string[] connectedFriends)
        {
            FriendsListView friendListView = new FriendsListView(connectedFriends); 
            NavigationService.Navigate(friendListView);
        }

        private void ConnectToService()
        {
            SpiderClueService.IFriendsManager friendsManager = new FriendsManagerClient(new System.ServiceModel.InstanceContext(this));
            friendsManager.Connect(UserSingleton.Instance.GamerTag);
        }
    }
}
