using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for ProfileEditionView.xaml
    /// </summary>
    public partial class ProfileEditionView : Page
    {
        public ProfileEditionView()
        {
            InitializeComponent();
            SetGamerIconInPage();
        }

        private void SetGamerIconInPage()
        {
            string iconPath = Utilities.GetImagePathForIcon();
            this.DataContext = new { ImagePath = iconPath };
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                if (UpdateData() == 1)
                {
                    ShowSuccessMessage();
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            
        }

        private void ShowSuccessMessage()
        {
            MessageBox.Show("Cambio realizado", Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show("Error en el cambio", Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool ValidateData()
        {
            String newName = txtName.Text;
            String newLastName = txtLastName.Text;
            bool nameValid = Validations.IsNameValid(newName);
            bool lastNameValid = Validations.IsNameValid(newLastName);

            if (!nameValid)
            {
                //lblInvalidName.Visibility = Visibility.Visible;
            }

            if (!lastNameValid)
            {
                //lblInvalidLastName.Visibility = Visibility.Visible;
            }

            return nameValid && lastNameValid;
        }

        private int UpdateData()
        {
            String newName = txtName.Text;
            String newLastName = txtLastName.Text;
            String gamertag = UserSingleton.Instance.GamerTag;
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            return userManager.ModifyAccount(gamertag, newName, newLastName);
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            GoToChangePasswordView();
        }

        private void GoToChangePasswordView()
        {
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            Gamer gamer = userManager.GetGamerByEmail(UserSingleton.Instance.Email);
            PasswordRecoveryView changePasswordView = new PasswordRecoveryView();
            changePasswordView.SetGamerInWindow(gamer);
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
