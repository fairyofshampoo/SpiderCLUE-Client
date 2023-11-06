using Spider_Clue.Views;
using System;
using System.Configuration;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace Spider_Clue.Logic
{
    internal class Utilities
    {
        private Utilities() { }
        public static string CalculateSHA1Hash(string textToHash)
        {
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(textToHash);
                byte[] hash = sha1.ComputeHash(bytes);

                StringBuilder textToHashBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    textToHashBuilder.Append(hash[i].ToString("x2"));
                }

                return textToHashBuilder.ToString();
            }
        }

        public static void PlayButtonClickSound()
        {
            SoundPlayer buttonClick = new SoundPlayer(Properties.Resources.MinecraftButtonSound);
            if (ConfigurationManager.AppSettings["SOUNDS_ON"].Equals("true"))
            {
                buttonClick.Play();
            }
        }

        public static void PlayMainThemeSong(MediaElement mediaElement)
        {
            if (ConfigurationManager.AppSettings["MUSIC_ON"].Equals("true"))
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

        private static string GetMainThemeSongPath()
        {
            string PathDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string PathProyectoDirectory = Path.GetFullPath(Path.Combine(PathDirectory, "../../../"));
            return PathProyectoDirectory + "Spider-Clue\\Audio\\MainMenuSong.wav";
        }

        public static bool SendEmailWithCode(string toEmail, Window mainWindow)
        {
            SpiderClueService.IEmailVerificationManager emailVerificationManager = new SpiderClueService.EmailVerificationManagerClient();
            emailVerificationManager.GenerateVerificationCode(toEmail);
            String codeToValidate = OpenDialogForEmailVerification(mainWindow);
            return emailVerificationManager.VerifyCode(toEmail, codeToValidate);
        }

        private static string OpenDialogForEmailVerification(Window mainWindow)
        {
            CodeInputDialog codeInputPopUp = new CodeInputDialog();
            codeInputPopUp.Owner = mainWindow;

            string codeFromInput = null;

            if (codeInputPopUp.ShowDialog() == true)
            {
                codeFromInput = codeInputPopUp.EmailValidation;
            }
            return codeFromInput;
        }
    }
}
