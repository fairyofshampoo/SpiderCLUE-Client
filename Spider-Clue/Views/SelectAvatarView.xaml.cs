using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

    public partial class SelectAvatarView : Page
    {
        private Image SelectedImage = null;
        private string newIconName = "Icon0.jpg";


        public SelectAvatarView()
        {
            InitializeComponent();
        }

        private void Image_Click(object sender, MouseButtonEventArgs e)
        {
            if (SelectedImage != null)
            {
                SelectedImage.Opacity = .5;
            }

            SelectedImage = (Image)sender;
            newIconName = SelectedImage.Name +".jpg";
            SelectedImage.Opacity = 1;
        }

        private void ChangeIcon()
        {
            LoggerManager logger = new LoggerManager(this.GetType());
            int result = 0;
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

            if (result == 1)
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