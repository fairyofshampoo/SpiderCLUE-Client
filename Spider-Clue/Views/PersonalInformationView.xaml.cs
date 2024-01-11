using Spider_Clue.Logic;
using System.Windows;
using System.Windows.Controls;

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
            Utilities.PlayButtonClickSound();
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
