using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Spider_Clue.Logic;
using System.Windows.Forms;

namespace Spider_Clue.Views
{
    public partial class MainMenuView : Page
    {
        public String ImagePath { get; set; }
        public MainMenuView()
        {
            InitializeComponent();
            Loaded += PageLoaded;
            Utilities.PlayMainThemeSong(mainThemePlayer);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            SetGamerData();
        }
        private void SetGamerData()
        {
            lblUserName.Content = UserSingleton.Instance.GamerTag;
            lblLevel.Content = UserSingleton.Instance.Level;
            ChangeImage();
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
    }
}
