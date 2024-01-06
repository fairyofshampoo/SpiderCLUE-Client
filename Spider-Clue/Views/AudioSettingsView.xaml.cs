using Spider_Clue.Logic;
using System;
using System.Configuration;
using System.Reflection;
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

        private const string MusicKey = "MUSIC_ON";
        private const string SoundKey = "SOUNDS_ON";

        public AudioSettingsView()
        {
            gameConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            musicStatus = gameConfiguration.AppSettings.Settings[MusicKey];
            soundStatus = gameConfiguration.AppSettings.Settings[SoundKey];
            InitializeComponent();
            SetMusicAndSoundSettings();
        }

        private void SetMusicAndSoundSettings()
        {
            Boolean isMusicOn = musicStatus.Value.Equals("true");
            btnMusicSettings.IsChecked = isMusicOn;
            ToggleMusicVisibility(isMusicOn);
            Boolean isSoundOn = soundStatus.Value.Equals("true");
            btnSoundSettings.IsChecked = isSoundOn;
            ToggleSoundVisibility(isSoundOn);
        }

        private void BtnMusic_Checked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            musicStatus.Value = "true";
            btnSoundSettings.IsChecked = false;
            ToggleMusicVisibility(true);
        }

        private void BtnMusic_Unchecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            musicStatus.Value = "false";
            ToggleMusicVisibility(false);
        }

        private void BtnSound_Checked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            soundStatus.Value = "true";
            ToggleSoundVisibility(true);
        }

        private void BtnSound_Unchecked(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            soundStatus.Value = "false";
            ToggleSoundVisibility(false);
        }

        private void SaveConfiguration()
        {
            gameConfiguration.Save();
            ConfigurationManager.RefreshSection("appSettings");
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


        private void ToggleMusicVisibility(bool isVisible)
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

        private void ToggleSoundVisibility(bool isVisible)
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
            string iconPath = Utilities.GetImagePathForImages() + "Icons\\" + iconFileName;
            imgMusicIcon.Source = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
        }

        private void SetSoundIcon(string iconFileName)
        {
            string iconPath = Utilities.GetImagePathForImages() + "Icons\\" + iconFileName;
            imgSoundIcon.Source = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
        }


    }
}
