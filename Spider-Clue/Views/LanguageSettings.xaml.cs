using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Spider_Clue.Logic;
using System.Configuration;
using System.Reflection;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for LanguageSettings.xaml
    /// </summary>
    public partial class LanguageSettings : Page
    {
        private readonly Configuration gameConfiguration;

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
        }

        private void BtnUSFlag_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            ChangeLanguage("en-US");
        }
    }
}
