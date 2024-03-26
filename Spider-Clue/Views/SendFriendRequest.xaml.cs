using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace Spider_Clue.Views
{

    public partial class SendFriendRequest : Window
    {
        public SendFriendRequest()
        {
            InitializeComponent();
        }

        private void ImgSearch_Click(object sender, MouseButtonEventArgs e)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                brSearchData.Visibility = Visibility.Visible;
                string gamertag = txtSearchGamer.Text;
                IUserManager userManager = new SpiderClueService.UserManagerClient();
                if (userManager.IsGamertagExisting(gamertag) == Constants.DefaultResultOperation)
                {
                    if (IsSearchValid(gamertag))
                    {
                        string icon = userManager.GetIcon(gamertag);
                        SetGamerData(gamertag, icon);
                        btnSendFriendRequest.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string icon = userManager.GetIcon(gamertag);
                        SetGamerData(gamertag, icon);
                    }
                }
                else
                {
                    ShowNotFoundMessage();
                }
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
        }

        private bool IsSearchValid(string friendGamertag)
        {
            return AreNotFriends(friendGamertag) && IsNotASelfFriendRequest(friendGamertag) && ThereIsNoFriendRequest(friendGamertag);
        }

        private bool AreNotFriends(string friendGamertag)
        {
            bool result = false;
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IFriendshipManager friendshipManager = new SpiderClueService.FriendshipManagerClient();
                result = friendshipManager.AreNotFriends(UserSingleton.Instance.GamerTag, friendGamertag);
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

        private bool ThereIsNoFriendRequest(string friendGamertag)
        {
            bool result = false;
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                SpiderClueService.IFriendshipManager friendshipManager = new SpiderClueService.FriendshipManagerClient();
                result = friendshipManager.ThereIsNoFriendRequest(UserSingleton.Instance.GamerTag, friendGamertag);
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

        private bool IsNotASelfFriendRequest(string friendGamertag)
        {
            bool result = false;
            if (friendGamertag != UserSingleton.Instance.GamerTag)
            {
                result = true;
            }
            return result;
        }

        private void ShowNotFoundMessage()
        {
            btnSendFriendRequest.Visibility = Visibility.Collapsed;
            lblGamertag.Content = Properties.Resources.ResultsNotFoundMessage;
            string iconPath = Utilities.GetImagePathForImages() + "Icons\\NotFoundIcon.png";
            this.DataContext = new { ImagePath = iconPath };
        }

        private void SetGamerData(string gamertag, string icon)
        {
            lblGamertag.Content = gamertag;
            string iconPath = Utilities.GetImagePathForImages() + "Avatars\\" + icon;
            this.DataContext = new { ImagePath = iconPath };
        }

        private void BtnSendFriendRequest_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                IFriendRequestManager friend = new SpiderClueService.FriendRequestManagerClient();
                friend.CreateFriendRequest(UserSingleton.Instance.GamerTag, lblGamertag.Content.ToString());
                this.Close();
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
        }

    }
}