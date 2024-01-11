using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Spider_Clue.Views
{

    public partial class SelectAvatarView : Page
    {
        private Image selectedImage = null;
        private string newIconName = "Icon0.jpg";


        public SelectAvatarView()
        {
            InitializeComponent();
        }

        private void Image_Click(object sender, MouseButtonEventArgs e)
        {
            if (selectedImage != null)
            {
                selectedImage.Opacity = .5;
            }

            selectedImage = (Image)sender;
            newIconName = selectedImage.Name +".jpg";
            selectedImage.Opacity = 1;
        }

        private void ChangeIcon()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            int result = Constants.DefaultResultOperation;
            try
            {
                UserSingleton.Instance.ImageCode = newIconName;
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                result = userManager.ChangeIcon(UserSingleton.Instance.GamerTag, newIconName);
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

            if (result == Constants.SuccessfulOperation)
            {
                DialogManager.ShowSuccessMessageBox(Properties.Resources.DlgSuccessfulChange);
            }
            else
            {
                DialogManager.ShowWarningMessageBox(Properties.Resources.DlgWrongChange);
            }
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ChangeIcon();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            ProfileEditionView profileEditionView = new ProfileEditionView();
            this.NavigationService.Navigate(profileEditionView);
        }
    }
}