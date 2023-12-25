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
using System.ServiceModel;
using System.Windows.Navigation;

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for SearchGameView.xaml
    /// </summary>
    public partial class SearchGameView : Window, IMatchManagerCallback
    {
        private readonly MatchManagerClient matchManagerClient;
        public static MainMenuForGuestView MenuGuestView { get; set; }
        public static MainMenuView MenuView { get; set; }
        public SearchGameView()
        {
            InitializeComponent();
            matchManagerClient = new MatchManagerClient(new InstanceContext(this));
        }

        private void Search_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SearchMatch();
        }

        private void SearchMatch()
        {
            bdrMatchFound.Visibility = Visibility.Visible;
            string matchCodeToSearch = txtMatchToSearch.Text;
            Match matchFound = matchManagerClient.GetMatchInformation(matchCodeToSearch);
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

        private void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            JoinMatch();
        }

        private void JoinMatch()
        {
            String matchCode = txtMatchToSearch.Text;
            String gamertag = UserSingleton.Instance.GamerTag;
            matchManagerClient.GetGamersInMatch(gamertag, matchCode);
        }

        public void ReceiveGamersInMatch(string[] gamertags)
        {
            int numberOfGamersInMatch = gamertags.Length;
            GoToLobby(numberOfGamersInMatch);
        }

        private void GoToLobby(int numberOfGamers)
        {
            int maximumOfPlayers = 3;
            int numberOfPlayersEmptyMatch = 0;

            if(numberOfGamers > numberOfPlayersEmptyMatch)
            {
                if (numberOfGamers < maximumOfPlayers)
                {
                    ChangeToLobbyView();
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(Properties.Resources.GameIsFullMessage, Properties.Resources.InformationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.GameHasEndedMessage, Properties.Resources.InformationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ChangeToLobbyView()
        {
            string matchCode = txtMatchToSearch.Text;
            LobbyView lobbyView = new LobbyView();
            lobbyView.SetMatchDataInPage(matchCode);

            if(UserSingleton.Instance.IsGuestPlayer)
            {
                MenuGuestView.NavigationService.Navigate(lobbyView);
            }
            else
            {
                MenuView.NavigationService.Navigate(lobbyView);
            }
        }
    }
}
