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
using Spider_Clue.SpiderClueService;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for SearchGameView.xaml
    /// </summary>
    public partial class SearchGameView : Window
    {
        public SearchGameView()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, MouseButtonEventArgs e)
        {
            SearchMatch();
            ShowMatchFounded();
            ShowSearchNotFoundAlert();
        }

        private void SearchMatch()
        {
            bdrMatchFound.Visibility = Visibility.Visible;
            string matchCodeToSearch = txtMatchToSearch.Text;
            SpiderClueService.IMatchManager matchManager = new SpiderClueService.MatchManagerClient();
            Match matchFound = matchManager.GetMatchInformation(matchCodeToSearch);
            if (matchFound != null)
            {
                lblCreator.Content = matchFound.CreatedBy;
                lblCode.Content = matchFound.Code;
            }
            else
            {
                bdrMatchFound.Visibility = Visibility.Collapsed;
                bdrNotFound.Visibility = Visibility.Visible;
            }
        }

        private void ShowMatchFounded()
        {

        }

        private void ShowSearchNotFoundAlert()
        {

        }

        private void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {
            JoinMatch();
        }

        private void JoinMatch()
        {

        }
    }
}
