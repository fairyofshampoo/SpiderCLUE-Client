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
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void LblLanguage_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            LanguageSettings languageSettings = new LanguageSettings();
            SettingsFrame.NavigationService.Navigate(languageSettings);
            LanguageSettings.settingsView = this;
        }

        private void LblAudio_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            AudioSettingsView audioSettingsView = new AudioSettingsView();  
            SettingsFrame.NavigationService.Navigate(audioSettingsView);
        }
    }
}
