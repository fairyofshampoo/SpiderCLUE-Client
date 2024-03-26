using log4net.Repository.Hierarchy;
using Spider_Clue.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Spider_Clue.Views
{
    public partial class AudioSettingsView : Page
    {
        private readonly Configuration gameConfiguration;
        private readonly KeyValueConfigurationElement musicStatus;
        private readonly KeyValueConfigurationElement soundStatus;

        public static SettingsView SettingsView { get; set; }
        public AudioSettingsView()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            InitializeComponent();
            try
            {
                gameConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                musicStatus = gameConfiguration.AppSettings.Settings[Constants.MusicKey];
                soundStatus = gameConfiguration.AppSettings.Settings[Constants.SoundKey];
                
                SetMusicAndSoundSettings();
            } 
            catch (ConfigurationErrorsException configurationException)
            {
                logger.LogError(configurationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgConfigurationException);
            }
            
        }

        private void SetMusicAndSoundSettings()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            try
            {
                bool isMusicOn = musicStatus.Value.Equals("true");
                tgbtnMusicSettings.IsChecked = isMusicOn;
                ChangeToggleMusicVisibility(isMusicOn);
                bool isSoundOn = soundStatus.Value.Equals("true");
                tgbtnSoundSettings.IsChecked = isSoundOn;
                ChangeToggleSoundVisibility(isSoundOn);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                logger.LogError(keyNotFoundException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgConfigurationException);
            }
        }

        private void TgbtnMusic_Checked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            musicStatus.Value = "true";
            tgbtnSoundSettings.IsChecked = false;
            ChangeToggleMusicVisibility(true);
        }

        private void TgbtnMusic_Unchecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            musicStatus.Value = "false";
            ChangeToggleMusicVisibility(false);
        }

        private void TgbtnSound_Checked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            soundStatus.Value = "true";
            ChangeToggleSoundVisibility(true);
        }

        private void TgbtnSound_Unchecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            soundStatus.Value = "false";
            ChangeToggleSoundVisibility(false);
        }

        private void SaveConfiguration()
        {
            string appConfigSection = "appSettings";
            gameConfiguration.Save();
            ConfigurationManager.RefreshSection(appConfigSection);
        }

        private void GoToMainMenuView()
        {
            MainMenuView mainMenuView = new MainMenuView();
            SettingsView.NavigationService.Navigate(mainMenuView);
        }

        private void GoToMainMenuGuestView()
        {
            MainMenuForGuestView mainMenuGuestView = new MainMenuForGuestView();
            SettingsView.NavigationService.Navigate(mainMenuGuestView);
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration();
            if (UserSingleton.Instance.IsGuestPlayer)
            {
                GoToMainMenuGuestView();
            }
            else
            {
                GoToMainMenuView();
            }
        }


        private void ChangeToggleMusicVisibility(bool isVisible)
        {
            if(isVisible)
            {
                SetMusicIcon("music-on.png");
            }
            else
            {
                SetMusicIcon("music-off.png");
            }
        }

        private void ChangeToggleSoundVisibility(bool isVisible)
        {
            if (isVisible)
            {
                SetSoundIcon("sound-on.png");
            }
            else
            {
                SetSoundIcon("sound-off.png");
            }
        }
        private void SetMusicIcon(string iconFileName)
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            try
            {
                string iconPath = Utilities.GetImagePathForImages() + "Icons\\" + iconFileName;
                imgMusicIcon.Source = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
            }
            catch (UriFormatException uriException)
            {
                logger.LogError(uriException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgIconException);
            }
        }

        private void SetSoundIcon(string iconFileName)
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            try
            {
                string iconPath = Utilities.GetImagePathForImages() + "Icons\\" + iconFileName;
                imgSoundIcon.Source = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
            }
            catch (UriFormatException uriException)
            {
                logger.LogError(uriException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgIconException);
            }
        }

    }
}
