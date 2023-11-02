using Spider_Clue.Logic;
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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
            String newName = txtName.Text;
            String newLastName = txtLastName.Text;
            String gamertag = UserSingleton.Instance.GamerTag;

            SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
            int modifyAccount = userManager.ModifyAccount (gamertag, newName, newLastName);
        }
    }
}
