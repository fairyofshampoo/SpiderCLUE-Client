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

    public partial class SettingsView : Page
    {
        public SettingsView()
        {
            InitializeComponent();
            Utilities.PlayMainThemeSong(mainThemePlayer);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (UserSingleton.Instance.IsGuestPlayer)
            {
                GoToMainMenuGuestView();
            }
            else
            {
                GoToMainMenuView();
            }
            
        }

        private void GoToMainMenuView()
        {
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void GoToMainMenuGuestView()
        {
            MainMenuForGuestView mainMenuGuestView = new MainMenuForGuestView();
            this.NavigationService.Navigate(mainMenuGuestView);
        }

        private void LblLanguage_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            LanguageSettings languageSettings = new LanguageSettings();
            SettingsFrame.NavigationService.Navigate(languageSettings);
            LanguageSettings.SettingsView = this;
        }

        private void LblAudio_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            AudioSettingsView audioSettingsView = new AudioSettingsView();  
            SettingsFrame.NavigationService.Navigate(audioSettingsView);
            AudioSettingsView.SettingsView = this;
        }
    }
}
