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
        private Dictionary<string, Pawn> gamersInGame;
        private string matchCode;
        private int diceNumber = 0;

        public GameBoardView()
        {
            InitializeComponent();
            GameManager = new GameManagerClient(new InstanceContext(this));
        }

        public void ConfigureWindow(string matchCode, Dictionary<string, Pawn> gamersInGame)
        {
            this.matchCode = matchCode;
            this.gamersInGame = gamersInGame;
            SetPawnInBoard();

            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                GameManager.ConnectGamerToGameBoard(UserSingleton.Instance.GamerTag, matchCode);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
                GoToMainMenu();
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
                GoToMainMenu();
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
                GoToMainMenu();
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
                GoToMainMenu();
            }
        }

        private void SetPawnInBoard()
        {
            string pawnColor = gamersInGame[UserSingleton.Instance.GamerTag].Color;
            switch (pawnColor)
            {
                case "BluePawn.png":
                    this.DataContext = new { PawnColor = "#5283AF" };
                    break;

                case "GreenPawn.png":
                    this.DataContext = new { PawnColor = "#1D4A24" };
                    break;

                case "PurplePawn.png":
                    this.DataContext = new { PawnColor = "#3E185D" };
                    break;

                case "RedPawn.png":
                    this.DataContext = new { PawnColor = "#920808" };
                    break;

                case "WhitePawn.png":
                    this.DataContext = new { PawnColor = "#F2EFEB" };
                    break;

                case "YellowPawn.png":
                    this.DataContext = new { PawnColor = "#FFBD59" };
                    break;

            }
        }

        private void GoToMainMenu()
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
        }

        private void Grid_Click(object sender, MouseButtonEventArgs mouseEvent)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                Point click = mouseEvent.GetPosition(dtgGameBoard);
                int columnClick = (int)(click.X / (int)dtgGameBoard.ActualWidth * dtgGameBoard.ColumnDefinitions.Count);
                int rowClick = (int)(click.Y / (int)dtgGameBoard.ActualHeight * dtgGameBoard.RowDefinitions.Count);
                dtgGameBoard.IsEnabled = false;
                GameManager.MovePawn(columnClick, rowClick, UserSingleton.Instance.GamerTag, matchCode);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
        }

        private void BtnLeaveGame_Click(object sender, RoutedEventArgs e)
        {
            LeaveGame();
        }

        public void LeaveGame()
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                GameManager.EndGameAsync(matchCode);
                GoToMainMenu();
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
        }

        private void BtnShowCards_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                Card[] cards = GameManager.GetDeck(UserSingleton.Instance.GamerTag);
                OpenDialogDeck(cards);
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
        }

        private void BtnRollDice_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                diceNumber = GameManager.RollDice(matchCode);
                OpenDialogRollDice(diceNumber);
                brRollDice.Visibility = Visibility.Collapsed;
                brDiceRoll.Visibility = Visibility.Visible;
                txtRollDice.Text = diceNumber.ToString();
            }
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
        }

        public void ReceivePawnsMove(Pawn pawn)
        {
            diceNumber = 0;
            switch (pawn.Color)
            {
                case "BluePawn.png":
                    Grid.SetRow(imgBluePawn, pawn.YPosition);
                    Grid.SetColumn(imgBluePawn, pawn.XPosition);
                    break;

                case "PurplePawn.png":
                    Grid.SetRow(imgPurplePawn, pawn.YPosition);
                    Grid.SetColumn(imgPurplePawn, pawn.XPosition);
                    break;

                case "WhitePawn.png":
                    Grid.SetRow(imgWhitePawn, pawn.YPosition);
                    Grid.SetColumn(imgWhitePawn, pawn.XPosition);
                    break;

                case "RedPawn.png":
                    Grid.SetRow(imgRedPawn, pawn.YPosition);
                    Grid.SetColumn(imgRedPawn, pawn.XPosition);
                    break;

                case "YellowPawn.png":
                    Grid.SetRow(imgYellowPawn, pawn.YPosition);
                    Grid.SetColumn(imgYellowPawn, pawn.XPosition);
                    break;

                case "GreenPawn.png":
                    Grid.SetRow(imgGreenPawn, pawn.YPosition);
                    Grid.SetColumn(imgGreenPawn, pawn.XPosition);
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
                    brRollDice.Visibility = Visibility.Visible;   
                }
                brTurnGreenColor.Visibility = Visibility.Visible;
                dtgGameBoard.IsEnabled = true;
            } else
            {
                brTurnGreenColor.Visibility = Visibility.Hidden;
                brRollDice.Visibility = Visibility.Collapsed;
                brDiceRoll.Visibility = Visibility.Collapsed;
                dtgGameBoard.IsEnabled = false;
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
            LoggerManager logger = new LoggerManager(this.GetType());

            try
            {
                if (isEnabled && ShowConfirmationFinalAccusation() == MessageBoxResult.OK)
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
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
        }

        private MessageBoxResult ShowConfirmationFinalAccusation()
        {
            return MessageBox.Show(Properties.Resources.DlgFinalAccusation, Properties.Resources.FinalAccusationTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        public void ReceiveCommonAccusationOption(bool isEnabled, Door door)
        {
            LoggerManager logger = new LoggerManager(this.GetType());

            try
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
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
            
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
            LoggerManager logger = new LoggerManager(this.GetType());

            try
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
            catch (EndpointNotFoundException endpointException)
            {
                logger.LogError(endpointException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgEndpointException);
            }
            catch (TimeoutException timeoutException)
            {
                logger.LogError(timeoutException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgTimeoutException);
            }
            catch (CommunicationException communicationException)
            {
                logger.LogError(communicationException);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgCommunicationException);
            }
            catch (Exception exception)
            {
                logger.LogFatal(exception);
                DialogManager.ShowErrorMessageBox(Properties.Resources.DlgFatalException);
            }
            
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
            MessageBox.Show(Properties.Resources.DlgNobodyAnswers, Properties.Resources.InformationTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}
