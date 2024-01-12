using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;


namespace Spider_Clue.Views
{
    public partial class DeckView : Window
    {
        private readonly Dictionary<string, string> imageDeckPaths;

        public DeckView(Card[] gamerDeck)
        {
            InitializeComponent();
            imageDeckPaths = new Dictionary<string, string>();
            ShowGamerDeck(gamerDeck);
        }

        public void ShowGamerDeck(Card[] gamerDeck)
        {
            List<Card> villainsDeck = GetVillainsDeck(gamerDeck);
            List<Card> motiveDeck = GetMotiveDeck(gamerDeck);
            List<Card> placeDeck = GetPlaceDeck(gamerDeck);

            if(villainsDeck.Any())
            {
                ShowVillainsCards(villainsDeck);
            }
            else
            {
                int index = 1;
                SetDefaultVillainCards(index);
            }

            if(motiveDeck.Any())
            {
                ShowMotiveCards(motiveDeck);
            }
            else
            {
                int index = 1;
                ShowMotiveDefaultCards(index);
            }

            if(placeDeck.Any())
            {
                ShowPlaceCards(placeDeck);
            }
            else
            {
                int index = 1;
                SetPlaceDefaultCards(index);
            }

        }

        public void ShowVillainsCards (List<Card> villainsDeck)
        {
            int index = 1;
            foreach (var card in villainsDeck.Select(card => card.ID))
            {
                switch (index)
                {
                    case 1:
                        brVillain1Path.Visibility = Visibility.Visible;
                        ShowDeck("Villain1Path", card);
                        break;

                    case 2:
                        brVillain2Path.Visibility = Visibility.Visible;
                        ShowDeck("Villain2Path", card);
                        break;

                    case 3:
                        brVillain3Path.Visibility = Visibility.Visible;
                        ShowDeck("Villain3Path", card);
                        break;

                    case 4:
                        brVillain4Path.Visibility = Visibility.Visible;
                        ShowDeck("Villain4Path", card);
                        break;

                    case 5:
                        brVillain5Path.Visibility = Visibility.Visible;
                        ShowDeck("Villain5Path", card);
                        break;

                    case 6:
                        brVillain6Path.Visibility = Visibility.Visible;
                        ShowDeck("Villain6Path", card);
                        break;
                }
                index++;
            }
            SetDefaultVillainCards(index);
        }

        public void SetDefaultVillainCards(int index)
        {
            int totalOfCards = 6;
            while(index <= totalOfCards)
            {
                switch (index)
                {
                    case 1:

                        ShowDeck("Villain1Path", "DefaultCard.png");
                        break;

                    case 2:
                        ShowDeck("Villain2Path", "DefaultCard.png");
                        break;

                    case 3:
                        ShowDeck("Villain3Path", "DefaultCard.png");
                        break;

                    case 4:
                        ShowDeck("Villain4Path", "DefaultCard.png");
                        break;

                    case 5:
                        ShowDeck("Villain5Path", "DefaultCard.png");
                        break;

                    case 6:
                        ShowDeck("Villain6Path", "DefaultCard.png");
                        break;
                }
                index++;
            }
        }

        public void ShowPlaceCards(List<Card> placeDeck)
        {
            int index = 1;
            foreach (var card in placeDeck.Select(card => card.ID))
            {
                switch (index)
                {
                    case 1:
                        brPlace1Path.Visibility = Visibility.Visible;
                        ShowDeck("Place1Path", card);
                        break;

                    case 2:
                        brPlace2Path.Visibility = Visibility.Visible;
                        ShowDeck("Place2Path", card);
                        break;

                    case 3:
                        brPlace3Path.Visibility = Visibility.Visible;
                        ShowDeck("Place3Path", card);
                        break;

                    case 4:
                        brPlace4Path.Visibility = Visibility.Visible;
                        ShowDeck("Place4Path", card);
                        break;

                    case 5:
                        brPlace5Path.Visibility = Visibility.Visible;
                        ShowDeck("Place5Path", card);
                        break;

                    case 6:
                        brPlace6Path.Visibility = Visibility.Visible;
                        ShowDeck("Place6Path", card);
                        break;
                }
                index++;
            }
            SetPlaceDefaultCards(index);
        }

        public void SetPlaceDefaultCards(int index)
        {
            int totalOfCards = 6;
            while (index <= totalOfCards) { 
                switch (index)
                {
                    case 1:
                        ShowDeck("Place1Path", "DefaultCard.png");
                        break;

                    case 2:
                        ShowDeck("Place2Path", "DefaultCard.png");
                        break;

                    case 3:
                        ShowDeck("Place3Path", "DefaultCard.png");
                        break;

                    case 4:
                        ShowDeck("Place4Path", "DefaultCard.png");
                        break;

                    case 5:
                        ShowDeck("Place5Path", "DefaultCard.png");
                        break;

                    case 6:
                        ShowDeck("Place6Path", "DefaultCard.png");
                        break;
                }
                index++;
            }
        }

        public void ShowMotiveCards(List<Card> motiveDeck)
        {
            int index = 1;
            foreach (var card in motiveDeck.Select(card => card.ID))
            {
                switch (index)
                {
                    case 1:
                        brMotive1Path.Visibility = Visibility.Visible;
                        ShowDeck("Motive1Path", card);
                        break;

                    case 2:
                        brMotive2Path.Visibility = Visibility.Visible;
                        ShowDeck("Motive2Path", card);
                        break;

                    case 3:
                        brMotive3Path.Visibility = Visibility.Visible;
                        ShowDeck("Motive3Path", card);
                        break;

                    case 4:
                        brMotive4Path.Visibility = Visibility.Visible;
                        ShowDeck("Motive4Path", card);
                        break;

                    case 5:
                        brMotive5Path.Visibility = Visibility.Visible;
                        ShowDeck("Motive5Path", card);
                        break;

                    case 6:
                        brMotive6Path .Visibility = Visibility.Visible;
                        ShowDeck("Motive6Path", card);
                        break;
                }
                index++;
            }
            ShowMotiveDefaultCards(index);
        }

        public void ShowMotiveDefaultCards (int index)
        {
            int totalOfCards = 6;
            while (index <= totalOfCards)
            {
                switch (index)
                {
                    case 1:
                        ShowDeck("Motive1Path", "DefaultCard.png");
                        break;

                    case 2:
                        ShowDeck("Motive2Path", "DefaultCard.png");
                        break;

                    case 3:
                        ShowDeck("Motive3Path", "DefaultCard.png");
                        break;

                    case 4:
                        ShowDeck("Motive4Path", "DefaultCard.png");
                        break;

                    case 5:
                        ShowDeck("Motive5Path", "DefaultCard.png");
                        break;

                    case 6:
                        ShowDeck("Motive6Path", "DefaultCard.png");
                        break;
                }
                index++;
            }
        }

        public List<Card> GetVillainsDeck(Card[] gamerDeck)
        {
            List<Card> villainsDeck = new List<Card>();
            foreach (Card card in gamerDeck)
            {
                if (card.Type == "Character")
                {
                    villainsDeck.Add(card);
                }
            }
            return villainsDeck;
        }

        public List<Card> GetMotiveDeck(Card[] gamerDeck)
        {
            List<Card> MotiveDeck = new List<Card>();
            foreach (Card card in gamerDeck)
            {
                if (card.Type == "Motive")
                {
                    MotiveDeck.Add(card);
                }
            }
            return MotiveDeck;
        }

        public List<Card> GetPlaceDeck(Card[] gamerDeck)
        {
            List<Card> MotiveDeck = new List<Card>();
            foreach (Card card in gamerDeck)
            {
                if (card.Type == "Place")
                {
                    MotiveDeck.Add(card);
                }
            }
            return MotiveDeck;
        }

        private void ShowDeck(string propertyName, string cardValue)
        {
            string cardPath = Utilities.GetImagePathForCards(cardValue);
            imageDeckPaths[propertyName] = cardPath;
            this.DataContext = imageDeckPaths;
        }

    }
}
