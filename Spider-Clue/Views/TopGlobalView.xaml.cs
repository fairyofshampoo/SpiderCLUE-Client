using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Controls;


namespace Spider_Clue.Views
{

    public partial class TopGlobalView : Page
    {
        public ObservableCollection<TopGlobal> TopGlobals { get; set; }

        public TopGlobalView(Winner[] top3Global)
        {
            InitializeComponent();
            TopGlobals = new ObservableCollection<TopGlobal>();
            dtgTopGlobal.ItemsSource = TopGlobals;
            ShowTopGlobal(top3Global);
        }

        private void ShowTopGlobal(Winner[] top3Global)
        {
            foreach (var top in top3Global)
            {
                string icon = Utilities.GetImagePathForIcon(top.Icon);
                TopGlobal topGlobal = new TopGlobal
                {
                Gamertag = top.Gamertag,
                Icon = icon,
                GamesWon = top.GamesWon.ToString()
                };
                TopGlobals.Add(topGlobal);
            }
        }

        public class TopGlobal
        {
            public string Gamertag { get; set; }
            public string Icon { get; set; }
            public string GamesWon { get; set; }
        }

        private void BtnGoBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (UserSingleton.Instance.IsGuestPlayer)
            {
                GoToMainMenuGuestView();
            }
            else
            {
                GoToMainMenuView();
            }
        }

        private void GoToMainMenuView()
        {
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void GoToMainMenuGuestView()
        {
            MainMenuForGuestView mainMenuGuestView = new MainMenuForGuestView();
            this.NavigationService.Navigate(mainMenuGuestView);
        }
    }
}
