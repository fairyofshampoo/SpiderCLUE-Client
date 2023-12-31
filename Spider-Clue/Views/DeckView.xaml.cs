using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Data;
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
                Console.WriteLine("Villanos");
                foreach(var card in villainsDeck)
                {
                    Console.WriteLine(card.ID);
                }
                ShowVillainsCards(villainsDeck);
            }

            if(motiveDeck.Any())
            {
                Console.WriteLine("Motivos");
                foreach (var card in motiveDeck)
                {
                    Console.WriteLine(card.ID);
                }
                ShowMotiveCards(motiveDeck);
            }

            if(placeDeck.Any())
            {
                Console.WriteLine("Lugares");
                foreach (var card in placeDeck)
                {
                    Console.WriteLine(card.ID);
                }
                ShowPlaceCards(placeDeck);
            }

        }

        public void ShowVillainsCards (List<Card> villainsDeck)
        {
            int index = 1;
            foreach (var card in villainsDeck)
            {
                switch (index)
                {
                    case 1:
                        ShowDeck("Villain1Path", card.ID);
                        break;

                    case 2:
                        ShowDeck("Villain2Path", card.ID);
                        break;

                    case 3:
                        ShowDeck("Villain3Path", card.ID);
                        break;

                    case 4:
                        ShowDeck("Villain4Path", card.ID);
                        break;

                    case 5:
                        ShowDeck("Villain5Path", card.ID);
                        break;

                    case 6:
                        ShowDeck("Villain6Path", card.ID);
                        break;
                }
                index++;
            }
        }

        public void ShowPlaceCards(List<Card> placeDeck)
        {
            int index = 1;
            foreach (var card in placeDeck)
            {
                switch (index)
                {
                    case 1:
                        ShowDeck("Place1Path", card.ID);
                        break;

                    case 2:
                        ShowDeck("Place2Path", card.ID);
                        break;

                    case 3:
                        ShowDeck("Place3Path", card.ID);
                        break;

                    case 4:
                        ShowDeck("Place4Path", card.ID);
                        break;

                    case 5:
                        ShowDeck("Place5Path", card.ID);
                        break;

                    case 6:
                        ShowDeck("Place6Path", card.ID);
                        break;
                }
                index++;
            }
        }

        public void ShowMotiveCards(List<Card> motiveDeck)
        {
            int index = 1;
            foreach (var card in motiveDeck)
            {
                switch (index)
                {
                    case 1:
                        ShowDeck("Motive1Path", card.ID);
                        break;

                    case 2:
                        ShowDeck("Motive2Path", card.ID);
                        break;

                    case 3:
                        ShowDeck("Motive3Path", card.ID);
                        break;

                    case 4:
                        ShowDeck("Motive4Path", card.ID);
                        break;

                    case 5:
                        ShowDeck("Motive5Path", card.ID);
                        break;

                    case 6:
                        ShowDeck("Motive6Path", card.ID);
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
