using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Spider_Clue.Views
{
    public partial class GameBoardView : Page , IGameManagerCallback
    {
        public readonly GameManagerClient GameManager;
        private Dictionary<string, Pawn> gamersInGame;
        private string matchCode;

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
            OpenDialogDeck();
        }

        private void BtnRollDice_Click(object sender, RoutedEventArgs e)
        {
           int rollDice = GameManager.RollDice(matchCode);

           OpenDialogRollDice(rollDice);
        }

        private void BtnAccuse_Click(object sender, RoutedEventArgs e)
        {
            //Realizar la acusación final 
            //Se tendrá que verificar cuál manda y ver si en el array es el mismo
            //Si sí es terminar la partida y guardar al usuario como ganador
            //Si no, deshabilia las funciones principales
        }

        public void ReceivePawnsMove(Pawn pawn)
        {
            Console.WriteLine("Peón");
            Console.WriteLine($"Pawn {pawn.Color}");
            Console.WriteLine($"Pawn {pawn.XPosition}");
            Console.WriteLine($"Pawn {pawn.YPosition}");

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

        public void OpenDialogDeck()
        {
            Window mainWindow = Window.GetWindow(this);
            DeckView deckView = new DeckView();
            deckView.Owner = mainWindow;
            deckView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            deckView.ShowDialog();
        }

        public void ReceiveTurn(bool isYourTurn)
        {
            if(isYourTurn)
            {
                btnRollDice.Visibility = Visibility.Visible;
                btnAccuse.Visibility = Visibility.Visible;
                GameBoardGrid.IsEnabled = true;
            } else
            {
                btnRollDice.Visibility = Visibility.Collapsed;
                btnAccuse.Visibility = Visibility.Collapsed;
                GameBoardGrid.IsEnabled = false;
            }
        }

        public void LeaveGameBoard()
        {
            GoToMainMenu();
        }

        public void ReceiveInvalidMove()
        {
            GameBoardGrid.IsEnabled = true;

            MessageBox.Show(Properties.Resources.DlgInvalidMove, Properties.Resources.InformationTitle);
        }
    }
}
