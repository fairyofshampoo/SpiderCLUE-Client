using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spider_Clue.Views
{

    public partial class ShowCommonAccusationView : Window
    {
        private Dictionary<string, string> imagePaths;

        public ShowCommonAccusationView(string[] cards)
        {
            InitializeComponent();
            imagePaths = new Dictionary<string, string>();
            ShowAccusationCards(cards);
        }

        private void ShowAccusationCards(string[] cards)
        {
            string place = cards[0];
            string sinister = cards[1];
            string motive = cards[2];

            ShowCard("PlaceCardPath", place);
            ShowCard("SinisterCardPath", sinister);
            ShowCard("MotiveCardPath", motive);
        }

        private void ShowCard(string propertyName, string cardValue)
        {
            string cardPath = Utilities.GetImagePathForCards(cardValue);
            imagePaths[propertyName] = cardPath;
            this.DataContext = imagePaths;
        }
    }
}
