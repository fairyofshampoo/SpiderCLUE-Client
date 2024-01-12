using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System.Collections.Generic;
using System.Windows;

namespace Spider_Clue.Views
{
    public partial class PassCardView : Window
    {
        private static readonly Dictionary<string, string> imageEvidencePath = new Dictionary<string, string>();
        public string TypeSelected { get; set; }

        public PassCardView(Card[] cards)
        {
            InitializeComponent();
            SelectCards(cards);
        }

        private void SelectCards(Card[] cards)
        {
            foreach(var card in cards)
            {
                switch(card.Type)
                {
                    case "Place":
                         btnPlace.Visibility = Visibility.Visible;
                         ShowCard("PlaceCardPath", card.ID);
                         break;

                    case "Character":
                        btnSinister.Visibility = Visibility.Visible;
                        ShowCard("SinisterCardPath", card.ID);
                        break;

                    case "Motive":
                        btnMotive.Visibility = Visibility.Visible;
                        ShowCard("MotiveCardPath", card.ID);
                        break;
                }
            }
            PrepareCards();
        }

        private void PrepareCards()
        {
            if (btnPlace.Visibility == Visibility.Collapsed)
            {
                ShowCard("PlaceCardPath", "DefaultCard.png");
            }
            else if (btnSinister.Visibility == Visibility.Collapsed)
            {
                ShowCard("SinisterCardPath", "DefaultCard.png");
            }
            else if (btnMotive.Visibility == Visibility.Collapsed)
            {
                ShowCard("MotiveCardPath", "DefaultCard.png");
            }
        }

        private void ShowCard(string propertyName, string cardValue)
        {
            string cardPath = Utilities.GetImagePathForCards(cardValue);
            if (!imageEvidencePath.ContainsKey(propertyName))
            {
                imageEvidencePath[propertyName] = cardPath;
            }
            this.DataContext = imageEvidencePath;
        }

        private void BtnMotiveCard_Click(object sender, RoutedEventArgs e)
        {
            string cardType = "Motive";
            GetCardSelected(cardType);
        }

        private void BtnPlaceCard_Click(object sender, RoutedEventArgs e)
        {
            string cardType = "Place";
            GetCardSelected(cardType);
        }

        private void BtnSiniesterCard_Click(object sender, RoutedEventArgs e)
        {
            string cardType = "Character";
            GetCardSelected(cardType);
        }

        private void GetCardSelected(string cardType)
        {
            TypeSelected = cardType;
            DialogResult = true;
            this.Close();
        }

    }
}
