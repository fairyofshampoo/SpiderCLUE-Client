using Spider_Clue.Logic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : Page
    { 
        public String SoundtrackPath { get; set; }
    
        public MainMenuView()
        {
            InitializeComponent();
            Loaded += PageLoaded;
            Soundtrack.Play();
            GetPath();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            SetGamerData();
        }

        private void SetGamerData()
        {
            lblUserName.Content = UserSingleton.Instance.GamerTag;
            lblLevel.Content = UserSingleton.Instance.Level;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsView settingsView = new SettingsView();
            this.NavigationService.Navigate(settingsView);
        }

        private void BtnJoinToParty_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogForSearchGame();
        }
        private void OpenDialogForSearchGame()
        {
            Window mainWindow = Window.GetWindow(this);

            SearchGameView searchGamePopUp = new SearchGameView();
            searchGamePopUp.Owner = mainWindow;
            searchGamePopUp.ShowDialog();
        }
        
        private void GetPath ()
        {
            string PathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string PathProyectoDirectory = Path.GetFullPath(Path.Combine(PathDirectory, "../../../"));
            SoundtrackPath = PathProyectoDirectory + "Spider-Clue\\Audio\\MainMenuSong.mp3";
            DataContext = this;
        }

        private void BtnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            PersonalInformationView personInformation = new PersonalInformationView();
            this.NavigationService.Navigate(personInformation);
        }
    }

}