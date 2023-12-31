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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spider_Clue.Views
{
    public partial class PassCardView : Window
    {
        private Dictionary<string, string> imageEvidencePath;
        public string TypeSelected { get; set; }

        public PassCardView(Card[] cards)
        {
            InitializeComponent();
            imageEvidencePath = new Dictionary<string, string>();
            ShowCards(cards);
        }

        private void ShowCards(Card[] cards)
        {
            foreach(var card in cards)
            {
                switch(card.Type)
                {
                    case "Place":
                         BtnPlace.Visibility = Visibility.Visible;
                         ShowCard("PlaceCardPath", card.ID);
                         break;

                    case "Character":
                        BtnSinister.Visibility = Visibility.Visible;
                        ShowCard("SinisterCardPath", card.ID);
                        break;

                    case "Motive":
                        BtnMotive.Visibility = Visibility.Visible;
                        ShowCard("MotiveCardPath", card.ID);
                        break;
                }
            }
        }

        private void ShowCard(string propertyName, string cardValue)
        {
            string cardPath = Utilities.GetImagePathForCards(cardValue);
            imageEvidencePath[propertyName] = cardPath;
            this.DataContext = imageEvidencePath;
        }

        private void BtnMotiveCard_Click(object sender, RoutedEventArgs e)
        {
            GetCardSelected("Motive");
        }

        private void BtnPlaceCard_Click(object sender, RoutedEventArgs e)
        {
            GetCardSelected("Place");
        }

        private void BtnSiniesterCard_Click(object sender, RoutedEventArgs e)
        {
            GetCardSelected("Character");
        }

        private void GetCardSelected(string cardType)
        {
            TypeSelected = cardType;
            DialogResult = true;
            this.Close();
        }

    }
}
