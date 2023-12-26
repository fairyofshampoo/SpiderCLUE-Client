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
    public partial class DeckView : Window
    {
        public DeckView()
        {
            InitializeComponent();
        }

        public void ShowGamerDeck()
        {
            Card[] gamerDeck = GetGamerDeck();
            List<Card> villainsDeck = GetVillainsDeck(gamerDeck);
            List<Card> motiveDeck = GetMotiveDeck(gamerDeck);
            List<Card> placeDeck = GetPlaceDeck(gamerDeck);
            if(villainsDeck != null)
            {
                ShowVillainsCards(villainsDeck);
            }

            if(motiveDeck != null)
            {
                ShowMotiveCards(motiveDeck);
            }

            if(placeDeck != null)
            {
                ShowPlaceCards(placeDeck);
            }

        }

        public void ShowVillainsCards (List<Card> villainsDeck)
        {
            int index = 1;
            foreach (var card in villainsDeck)
            {
                string cardPath = Utilities.GetImagePathForCards(card.ID);
                switch (index)
                {
                    case 1:
                        this.DataContext = new { Villain1Path = cardPath };
                        break;

                    case 2:
                        this.DataContext = new { Villain2Path = cardPath };
                        break;

                    case 3:
                        this.DataContext = new { Villain3Path = cardPath };
                        break;

                    case 4:
                        this.DataContext = new { Villain4Path = cardPath };
                        break;

                    case 5:
                        this.DataContext = new { Villain5Path = cardPath };
                        break;

                    case 6:
                        this.DataContext = new { Villain6Path = cardPath };
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
                string cardPath = Utilities.GetImagePathForCards(card.ID);
                switch (index)
                {
                    case 1:
                        this.DataContext = new { Place1Path = cardPath };
                        break;

                    case 2:
                        this.DataContext = new { Place2Path = cardPath };
                        break;

                    case 3:
                        this.DataContext = new { Place3Path = cardPath };
                        break;

                    case 4:
                        this.DataContext = new { Place4Path = cardPath };
                        break;

                    case 5:
                        this.DataContext = new { Place5Path = cardPath };
                        break;

                    case 6:
                        this.DataContext = new { Place6Path = cardPath };
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
                string cardPath = Utilities.GetImagePathForCards(card.ID);
                switch (index)
                {
                    case 1:
                        this.DataContext = new { Motive1Path = cardPath };
                        break;

                    case 2:
                        this.DataContext = new { Motive2Path = cardPath };
                        break;

                    case 3:
                        this.DataContext = new { Motive3Path = cardPath };
                        break;

                    case 4:
                        this.DataContext = new { Motive4Path = cardPath };
                        break;

                    case 5:
                        this.DataContext = new { Motive5Path = cardPath };
                        break;

                    case 6:
                        this.DataContext = new { Motive6Path = cardPath };
                        break;
                }
                index++;
            }
        }

        public Card [] GetGamerDeck()
        {
            SpiderClueService.ICardManager cardManager = new SpiderClueService.CardManagerClient();
            Card [] deck = cardManager.GetDeck(UserSingleton.Instance.GamerTag);
            return deck;
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

    }
}
