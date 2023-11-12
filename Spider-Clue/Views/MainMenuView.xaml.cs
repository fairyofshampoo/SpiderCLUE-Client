using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        public String ImagePath { get; set; }
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
            Utilities.SetUserIcon(GamerIcon);
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
            searchGamePopUp.ShowDialog();
        }

        private void BtnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            PersonalInformationView personInformation = new PersonalInformationView();
            NavigationService.Navigate(personInformation);
        }

        private void ChangeImage()
        {
            string PathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string PathProyectoDirectory = Path.GetFullPath(Path.Combine(PathDirectory, "../../../"));
            ImagePath = PathProyectoDirectory + "Spider-Clue\\Images\\" + UserSingleton.Instance.ImageCode;
            DataContext = this;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            //meter un logout
            App.Current.Shutdown();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            CreateMatch();
            GoToLobbyView();
        }

        private void CreateMatch()
        {
            
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
            //Debe mostrra los amigos conectados
        }

        private void ConnectToService()
        {
            SpiderClueService.IFriendsManager friendsManager = new FriendsManagerClient(new System.ServiceModel.InstanceContext(this));
            friendsManager.Connect(UserSingleton.Instance.GamerTag);
        }
    }
}
