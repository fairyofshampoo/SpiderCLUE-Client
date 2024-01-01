using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Spider_Clue.Views
{
    public partial class GameBoardView : Page , IGameManagerCallback
    {
        public readonly GameManagerClient GameManager;
        public readonly SpiderClueService.ICardManager CardManager;
        private Dictionary<string, Pawn> gamersInGame;
        private string matchCode;
        private int diceNumber = 0;

        public GameBoardView()
        {
            InitializeComponent();
            GameManager = new GameManagerClient(new InstanceContext(this));
            CardManager = new SpiderClueService.CardManagerClient();
        }

        public void ConfigureWindow(string matchCode, Dictionary<string, Pawn> gamersInGame)
        {
            this.matchCode = matchCode;
            this.gamersInGame = gamersInGame;
            SetPawnInBoard();

            try
            {
                GameManager.ConnectGamerToGameBoard(UserSingleton.Instance.GamerTag, matchCode);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.DlgCommunicationException, Properties.Resources.ErrorTitle);
                GoToMainMenu();
            }
        }

        private void SetPawnInBoard()
        {
            string pawnColor = gamersInGame[UserSingleton.Instance.GamerTag].Color;
            txtblckPawnColor.Text = "Tu peón es: " + pawnColor;
        }

        public void GoToMainMenu()
        {
            if (UserSingleton.Instance.IsGuestPlayer)
            {
                MainMenuForGuestView mainMenuForGuestView = new MainMenuForGuestView();
                this.NavigationService.Navigate(mainMenuForGuestView);
            }
            else
            {
                MainMenuView mainMenuView = new MainMenuView();
                this.NavigationService.Navigate(mainMenuView);
            }
            GameManager.DisconnectFromBoardAsync(UserSingleton.Instance.GamerTag, matchCode);
        }

        private void Grid_Click(object sender, MouseButtonEventArgs mouseEvent)
        {
            Point click = mouseEvent.GetPosition(GameBoardGrid);
            int columnClick = (int)(click.X / (int)GameBoardGrid.ActualWidth * GameBoardGrid.ColumnDefinitions.Count);
            int rowClick = (int)(click.Y / (int)GameBoardGrid.ActualHeight * GameBoardGrid.RowDefinitions.Count);
            GameBoardGrid.IsEnabled = false;
            GameManager.MovePawn(columnClick, rowClick, UserSingleton.Instance.GamerTag, matchCode);
        }

        private void BtnLeaveGame_Click(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();
        }

        private void BtnShowCards_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            SpiderClueService.ICardManager cardManager = new SpiderClueService.CardManagerClient();
            Card[] cards = GameManager.GetDeck(UserSingleton.Instance.GamerTag);
            Console.WriteLine("El mazo es:");
            foreach(var deck in cards)
            {
                Console.WriteLine(deck.ID);
                Console.WriteLine("-----");
            }
            OpenDialogDeck(cards);
        }

        private void BtnRollDice_Click(object sender, RoutedEventArgs e)
        {
            diceNumber = GameManager.RollDice(matchCode);
            OpenDialogRollDice(diceNumber);
            btnRollDice.Visibility = Visibility.Collapsed;
        }

        public void ReceivePawnsMove(Pawn pawn)
        {
            diceNumber = 0;
            switch (pawn.Color)
            {
                case "BluePawn.png":
                    Grid.SetRow(bluePawn, pawn.YPosition);
                    Grid.SetColumn(bluePawn, pawn.XPosition);
                    break;

                case "PurplePawn.png":
                    Grid.SetRow(purplePawn, pawn.YPosition);
                    Grid.SetColumn(purplePawn, pawn.XPosition);
                    break;

                case "WhitePawn.png":
                    Grid.SetRow(whitePawn, pawn.YPosition);
                    Grid.SetColumn(whitePawn, pawn.XPosition);
                    break;

                case "RedPawn.png":
                    Grid.SetRow(redPawn, pawn.YPosition);
                    Grid.SetColumn(redPawn, pawn.XPosition);
                    break;

                case "YellowPawn.png":
                    Grid.SetRow(yellowPawn, pawn.YPosition);
                    Grid.SetColumn(yellowPawn, pawn.XPosition);
                    break;

                case "GreenPawn.png":
                    Grid.SetRow(greenPawn, pawn.YPosition);
                    Grid.SetColumn(greenPawn, pawn.XPosition);
                    break; 
            }
        }

        public void OpenDialogRollDice(int diceRoll) 
        {
            Window mainWindow = Window.GetWindow(this);
            RollDiceView rollDiceView = new RollDiceView(diceRoll);
            rollDiceView.Owner = mainWindow;
            rollDiceView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            rollDiceView.ShowDialog();
        }

        public void OpenDialogDeck(Card[] deck)
        {
            Window mainWindow = Window.GetWindow(this);
            DeckView deckView = new DeckView(deck);
            deckView.Owner = mainWindow;
            deckView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            deckView.ShowDialog();
        }

        public void ReceiveTurn(bool isYourTurn)
        {
            if(isYourTurn)
            {
                if (diceNumber == 0)
                {
                    btnRollDice.Visibility = Visibility.Visible;
                }
                GameBoardGrid.IsEnabled = true;
            } else
            {
                btnRollDice.Visibility = Visibility.Collapsed;
                GameBoardGrid.IsEnabled = false;
            }
        }

        public void LeaveGameBoard()
        {
            GoToMainMenu();
        }

        public void ReceiveInvalidMove()
        {
            ReceiveTurn(true);
            MessageBox.Show(Properties.Resources.DlgInvalidMove, Properties.Resources.InformationTitle);
        }

        public void ReceiveFinalAccusationOption(bool isEnabled)
        {
            if(isEnabled)
            {
                if (ShowConfirmationFinalAccusation() == MessageBoxResult.OK)
                {
                    string sinister = OpenDialogSixSinistersCard();
                    string motive = OpenDialogMotiveCard();
                    string place = OpenDialogPlaceCard();
                    string[] cards = new string[3];
                    cards[0] = sinister;
                    cards[1] = motive;
                    cards[2] = place;
                    GameManager.MakeFinalAccusation(cards, matchCode, UserSingleton.Instance.GamerTag);
                }
            }
        }

        private MessageBoxResult ShowConfirmationFinalAccusation()
        {
            //Cambiar el mensaje para que pregunte si quiere realizar la acusación final
            return MessageBox.Show(Properties.Resources.DlgConfirmDeleteFriend, Properties.Resources.DeleteFriendTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public void ReceiveCommonAccusationOption(bool isEnabled, Door door)
        {
            string sinister = OpenDialogSixSinistersCard();
            string motive = OpenDialogMotiveCard();
            string place = door.ZoneName;

            string[] cards = new string[3];
            cards[0] = place;
            cards[1] = sinister;
            cards[2] = motive;
            GameManager.ShowCommonAccusation(cards, matchCode, UserSingleton.Instance.GamerTag);
        }

        public void ReceiveCommonAccusationByOtherGamer(string[] accusation)
        {
            OpenDialogCommonAccusationByOtherGamer(accusation);
        }

        public void ReceiveCardAccused(Card card)
        {
            OpenDialogShowEvidence(card.ID);
        }

        public void RequestShowCard(Card[] cards, string accuser)
        {
            string typeSelected = OpenDialogPassCard(cards);
            Card selectedCard = new Card();
            foreach (var card in cards)
            {
                if (card.Type == typeSelected)
                {
                    selectedCard = card; break;
                }
            }
            GameManager.ShowCard(selectedCard, matchCode, accuser);
        }

        public void ReceiveWinner(string winnerGamertag, string gamerIcon)
        {
            OpenDialogShowWinner(winnerGamertag, gamerIcon);
        }

        public void OpenDialogShowWinner(string winnerGamertag, string gamerIcon)
        {
            Window mainWindow = Window.GetWindow(this);
            ShowWinner showWinner = new ShowWinner(winnerGamertag, gamerIcon);
            showWinner.Owner = mainWindow;
            showWinner.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            showWinner.Show();
        }

        public void OpenDialogShowEvidence(string cardID)
        {
            Window mainWindow = Window.GetWindow(this);
            ShowEvidenceView evidenceView = new ShowEvidenceView(cardID);
            evidenceView.Owner = mainWindow;
            evidenceView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            evidenceView.Show();
        }

        public void OpenDialogCommonAccusationByOtherGamer(string[] accusation)
        {
            Window mainWindow = Window.GetWindow(this);
            ShowCommonAccusationView commonAccusation = new ShowCommonAccusationView(accusation);
            commonAccusation.Owner = mainWindow;
            commonAccusation.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            commonAccusation.Show();
        }

        public string OpenDialogPassCard(Card[] card)
        {
            Window mainWindow = Window.GetWindow(this);
            PassCardView passCard = new PassCardView(card);
            passCard.Owner = mainWindow;
            passCard.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            string typeSelected = string.Empty;
            if (passCard.ShowDialog() == true)
            {
                typeSelected = passCard.TypeSelected;
            }
            return typeSelected;
        }

        public string OpenDialogSixSinistersCard()
        {
            Window mainWindow = Window.GetWindow(this);
            SinisterSixCardsView sinisterSixCards = new SinisterSixCardsView();
            sinisterSixCards.Owner = mainWindow;
            sinisterSixCards.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            string sinisterCard = string.Empty;
            if(sinisterSixCards.ShowDialog() == true)
            {
                sinisterCard = sinisterSixCards.SinisterCard;
            }
            return sinisterCard;
        }

        public string OpenDialogMotiveCard()
        {
            Window mainWindow = Window.GetWindow(this);
            ReasonForFailureCardsView reasonForFailure = new ReasonForFailureCardsView();
            reasonForFailure.Owner = mainWindow;
            reasonForFailure.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            string motiveCard = string.Empty;
            if (reasonForFailure.ShowDialog() == true)
            {
                motiveCard = reasonForFailure.ReasonCard;
            }
            return motiveCard;
        }

        public string OpenDialogPlaceCard()
        {
            Window mainWindow = Window.GetWindow(this);
            PlaceOfFailureCardsView placeOfFailure = new PlaceOfFailureCardsView();
            placeOfFailure.Owner = mainWindow;
            placeOfFailure.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            string placeCard = string.Empty;
            if (placeOfFailure.ShowDialog() == true)
            {
                placeCard = placeOfFailure.PlaceCard;
            }
            return placeCard;
        }

        public void ShowNobodyAnswers()
        {
            //Cambiar mensajito
            MessageBox.Show(Properties.Resources.DlgConfirmDeleteFriend, Properties.Resources.DeleteFriendTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}
