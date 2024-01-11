using System.Windows;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.Configuration;
using System.Reflection;
using log4net.Repository.Hierarchy;

namespace Spider_Clue.Views
{
    public partial class LanguageSettings : Page
    {
        private readonly Configuration gameConfiguration;
        public static SettingsView SettingsView { get; set; }

        public LanguageSettings()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            InitializeComponent();
            try
            {
                gameConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            }
            catch (ConfigurationErrorsException configurationException)
            {
                logger.LogError(configurationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgConfigurationException);
            }
        }

        private void ChangeLanguage(string language)
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            string appConfigSection = "appSettings";

            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
                gameConfiguration.Save();
                ConfigurationManager.RefreshSection(appConfigSection);
            }
            catch (ConfigurationErrorsException configurationException)
            {
                logger.LogError(configurationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgConfigurationException);
            }
            
        }

        private void BtnMXFlag_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            string mexicanSpanish = "es-MX";
            ChangeLanguage(mexicanSpanish);
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
            string americanEnglish = "en-US";
            ChangeLanguage(americanEnglish);
            ReloadSettingsPage();
        }
    }
}
