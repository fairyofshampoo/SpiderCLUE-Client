using Microsoft.Win32;
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
    /// Interaction logic for PersonalInformationView.xaml
    /// </summary>
    public partial class PersonalInformationView : Page
    {
        public PersonalInformationView()
        {
            InitializeComponent();
        }

        private void btnChangeInformation_Click(object sender, RoutedEventArgs e)
        {
            ProfileEditionView profileEditionView = new ProfileEditionView();
            this.NavigationService.Navigate(profileEditionView);
        }
    }
}
