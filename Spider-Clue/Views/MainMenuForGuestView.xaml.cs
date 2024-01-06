using Spider_Clue.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Spider_Clue.Views
{
    public partial class MainMenuForGuestView : Page
    {
        public MainMenuForGuestView()
        {
            InitializeComponent();
            Loaded += PageLoaded;
            Utilities.PlayMainThemeSong(meMainThemePlayer);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            SetGamerData();
        }
        private void SetGamerData()
        {
            lblUserName.Content = UserSingleton.Instance.GamerTag;
            string iconPath = Utilities.GetImagePathForIcon(UserSingleton.Instance.ImageCode);
            this.DataContext = new { ImagePath = iconPath };
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
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
            SearchGameView.MenuGuestView = this;
            searchGamePopUp.ShowDialog();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SettingsView settingsView = new SettingsView();
            NavigationService.Navigate(settingsView);

        }
    }
}
