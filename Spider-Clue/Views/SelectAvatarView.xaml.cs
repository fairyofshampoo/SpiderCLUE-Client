using Spider_Clue.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Interaction logic for SelectAvatarView.xaml
    /// </summary>
    public partial class SelectAvatarView : Page
    {
        private Image SelectedImage = null;
        private string newIconName = "Icon0";


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
            newIconName = SelectedImage.Name;
            SelectedImage.Opacity = 1;
        }

        private void ChangeIcon()
        {
            UserSingleton.Instance.ImageCode = SelectedImage.Name;
            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            userManager.ChangeIcon(UserSingleton.Instance.GamerTag, newIconName);
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ChangeIcon();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
