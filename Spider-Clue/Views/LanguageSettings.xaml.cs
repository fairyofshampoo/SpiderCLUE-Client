using System.Windows;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.Configuration;
using System.Reflection;

namespace Spider_Clue.Views
{
    public partial class LanguageSettings : Page
    {
        private readonly Configuration gameConfiguration;
        public static SettingsView SettingsView { get; set; }

        public LanguageSettings()
        {
            gameConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            InitializeComponent();
        }

        private void ChangeLanguage(string language)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            gameConfiguration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void BtnMXFlag_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            ChangeLanguage("es-MX");
            ReloadSettingsPage();
        }
        private void ReloadSettingsPage()
        {
            SettingsView newSettingsView = new SettingsView();
            SettingsView.NavigationService.Navigate(newSettingsView);

        }

        private void BtnUSFlag_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            ChangeLanguage("en-US");
            ReloadSettingsPage();
        }
    }
}
