using Spider_Clue.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;


namespace Spider_Clue.Views
{

    public partial class Top3GlobalView : Page
    {
        public ObservableCollection<TopGlobal> TopGlobals { get; set; }

        public Top3GlobalView()
        {
            InitializeComponent();
            TopGlobals = new ObservableCollection<TopGlobal>();
            Top3GlobalGrid.ItemsSource = TopGlobals;
            ShowTop3Global();
        }

        private void ShowTop3Global()
        {
            SpiderClueService.IWinnersManager winners = new SpiderClueService.WinnersManagerClient();
            var topGlobals = winners.GetTopGlobalWinners();
            if(topGlobals.Any())
            {
                foreach (var top in topGlobals)
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
        }

        public class TopGlobal
        {
            public string Gamertag { get; set; }
            public string Icon { get; set; }
            public string GamesWon { get; set; }
        }
    }
}
