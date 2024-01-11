using Spider_Clue.Views;
using System;
using System.Configuration;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ServiceModel;


namespace Spider_Clue.Logic
{
    internal class Utilities
    {
        private Utilities() { }
        public static string CalculateSHA1Hash(string textToHash)
        {
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                string hexadecimalFormat = "x2";
                byte[] bytes = Encoding.UTF8.GetBytes(textToHash);
                byte[] hash = sha1.ComputeHash(bytes);

                StringBuilder textToHashBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    textToHashBuilder.Append(hash[i].ToString(hexadecimalFormat));
                }

                return textToHashBuilder.ToString();
            }
        }

        public static void PlayButtonClickSound()
        {
            SoundPlayer buttonClick = new SoundPlayer(Properties.Resources.MinecraftButtonSound);
            if (ConfigurationManager.AppSettings[Constants.SoundKey].Equals("true"))
            {
                buttonClick.Play();
            }
        }

        public static void PlayMainThemeSong(MediaElement mediaElement)
        {
            LoggerManager logger = new LoggerManager(mediaElement.GetType());
            try
            {
                if (ConfigurationManager.AppSettings[Constants.MusicKey].Equals("true"))
                {
                    mediaElement.Source = new Uri(GetMainThemeSongPath(), UriKind.RelativeOrAbsolute);
                    mediaElement.MediaEnded += (sender, e) =>
                    {
                        mediaElement.Position = TimeSpan.Zero;
                        mediaElement.Play();
                    };
                    mediaElement.Play();
                }
            }
            catch (UriFormatException uriFormatException)
            {
                logger.LogError(uriFormatException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgMusicException);
            }
        }

        private static string GetMainThemeSongPath()
        {
            string path = string.Empty;
            LoggerManager logger = new LoggerManager(AppDomain.CurrentDomain.BaseDirectory.GetType());
            try
            {
                string PathDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string PathProyectoDirectory = Path.GetFullPath(Path.Combine(PathDirectory, "../../../"));
                path = PathProyectoDirectory + "Spider-Clue\\Audio\\MainMenuSong.wav";

            } 
            catch (PathTooLongException pathTooLongException)
            {
                logger.LogError(pathTooLongException);
            }

            return path;
        }

        public static string GetImagePathForIcon(string imageCode)
        {
            return GetImagePathForImages() + "Avatars\\" + imageCode;
        }

        public static string GetImagePathForImages()
        {
            string path = string.Empty;
            LoggerManager logger = new LoggerManager(AppDomain.CurrentDomain.BaseDirectory.GetType());
            try
            {
                string PathDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string PathProyectoDirectory = Path.GetFullPath(Path.Combine(PathDirectory, "../../../"));
                path = PathProyectoDirectory + "Spider-Clue\\Images\\";
            }
            catch (PathTooLongException pathTooLongException)
            {
                logger.LogError(pathTooLongException);
            }

            return path;
        }

        public static string GetImagePathForCards(string imageCode)
        {
            return GetImagePathForImages() + "GameCards\\" + imageCode;
        }

        public static bool SendEmailWithCode(string toEmail, Window mainWindow)
        {
            bool result = false;
            LoggerManager logger = new LoggerManager(mainWindow.GetType());

            try
            {
                SpiderClueService.IEmailVerificationManager emailVerificationManager = new SpiderClueService.EmailVerificationManagerClient();
                emailVerificationManager.GenerateVerificationCode(toEmail);
                string codeToValidate = OpenDialogForEmailVerification(mainWindow);
                result = emailVerificationManager.VerifyCode(toEmail, codeToValidate);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
            return result;
        }

        private static string OpenDialogForEmailVerification(Window mainWindow)
        {
            CodeInputDialog codeInputPopUp = new CodeInputDialog();
            codeInputPopUp.Owner = mainWindow;
            codeInputPopUp.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            string codeFromInput = null;

            if (codeInputPopUp.ShowDialog() == true)
            {
                codeFromInput = codeInputPopUp.EmailValidation;
            }
            return codeFromInput;
        }
    }
}
