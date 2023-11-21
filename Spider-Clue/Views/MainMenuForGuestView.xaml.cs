using Spider_Clue.Logic;
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

namespace Spider_Clue.Views
{
    public partial class MainMenuForGuestView : Page
    {
        public MainMenuForGuestView()
        {
            InitializeComponent();
            Loaded += PageLoaded;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            SetGamerData();
        }
        private void SetGamerData()
        {
            lblUserName.Content = UserSingleton.Instance.GamerTag;
            string iconPath = Utilities.GetImagePathForIcon();
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
