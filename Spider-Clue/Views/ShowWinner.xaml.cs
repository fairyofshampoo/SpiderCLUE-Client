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
using System.Windows.Shapes;

namespace Spider_Clue.Views
{
    public partial class ShowWinner : Window
    {
        public ShowWinner(string gamertag, string gamerIcon)
        {
            InitializeComponent();
            ShowWinnerData(gamertag, gamerIcon);
        }

        public void ShowWinnerData(string gamertag, string gamerIcon)
        {
            lblUserName.Content = gamertag;
            string iconPath = Utilities.GetImagePathForIcon(gamerIcon);
            this.DataContext = new { UserIconPath = iconPath };
        }

    }
}
