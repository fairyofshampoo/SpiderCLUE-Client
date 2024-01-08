using Spider_Clue.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            frSettings.NavigationService.Navigate(languageSettings);
            LanguageSettings.SettingsView = this;
        }

        private void LblAudio_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            AudioSettingsView audioSettingsView = new AudioSettingsView();  
            frSettings.NavigationService.Navigate(audioSettingsView);
            AudioSettingsView.SettingsView = this;
        }
    }
}
