using Microsoft.Win32;
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
    public partial class PersonalInformationView : Page
    {
        public PersonalInformationView()
        {
            InitializeComponent();
            ShowGamerData();
        }

        private void BtnChangeInformation_Click(object sender, RoutedEventArgs e)
        {
            ProfileEditionView profileEditionView = new ProfileEditionView();
            this.NavigationService.Navigate(profileEditionView);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            this.NavigationService.GoBack();
        }

        private void ShowGamerData()
        {
            txtEmail.Text = UserSingleton.Instance.Email;
            txtGamerTag.Text = UserSingleton.Instance.GamerTag;
            txtLastName.Text = UserSingleton.Instance.LastName;
            txtName.Text = UserSingleton.Instance.Name;
        }
    }
}
