using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Spider_Clue.Views
{
    public partial class ProfileEditionView : Page
    {
        public ProfileEditionView()
        {
            InitializeComponent();
            SetGamerIconInPage();
        }

        private void SetGamerIconInPage()
        {
            string iconPath = Utilities.GetImagePathForIcon(UserSingleton.Instance.ImageCode);
            this.DataContext = new { ImagePath = iconPath };
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();

            if (ValidateData())
            {
                if (UpdateData() == 1)
                {
                    DialogManager.ShowSuccessMessageBox(Properties.Resources.DlgSuccessfulChange);
                }
                else
                {
                    DialogManager.ShowWarningMessageBox(Properties.Resources.DlgWrongChange);
                }
            }
            
        }

        private bool ValidateData()
        {
            string newName = txtName.Text;
            string newLastName = txtLastName.Text;
            bool nameValid = Validations.IsNameValid(newName);
            bool lastNameValid = Validations.IsNameValid(newLastName);

            if (!nameValid)
            {
                lblInvalidName.Visibility = Visibility.Visible;
            }

            if (!lastNameValid)
            {
                lblInvalidLastName.Visibility = Visibility.Visible;
            }

            return nameValid && lastNameValid;
        }

        private int UpdateData()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            int result = 0;
            try
            {
                string newName = txtName.Text;
                string newLastName = txtLastName.Text;
                string gamertag = UserSingleton.Instance.GamerTag;
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                result = userManager.ModifyAccount(gamertag, newName, newLastName);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                result = 0;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                result = 0;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                result = 0;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                result = 0;
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }

            return result;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            GoToChangePasswordView();
        }

        private void GoToChangePasswordView()
        {
            PasswordRecoveryView changePasswordView = new PasswordRecoveryView();
            changePasswordView.SetGamertagInWindow(UserSingleton.Instance.GamerTag);
            this.NavigationService.Navigate(changePasswordView);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void LblChangeAvatar_Click(object sender, MouseButtonEventArgs e)
        {
            SelectAvatarView selectAvatarView = new SelectAvatarView();
            this.NavigationService.Navigate(selectAvatarView);
        }
    }
}
