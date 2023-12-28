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
            //SetPawnsInBoard();

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
            GameManager.DisconnectFromBoardAsync(UserSingleton.Instance.GamerTag, matchCode);
        }

        private void Grid_Click(object sender, MouseButtonEventArgs mouseEvent)
        {
            Point click = mouseEvent.GetPosition(GameBoardGrid);
            int columnClick = (int)(click.X / (int)GameBoardGrid.ActualWidth * GameBoardGrid.ColumnDefinitions.Count);
            int rowClick = (int)(click.Y / (int)GameBoardGrid.ActualHeight * GameBoardGrid.RowDefinitions.Count);

            GameManager.MovePawn(columnClick, rowClick, UserSingleton.Instance.GamerTag);
        }

        private void BtnLeaveGame(object sender, RoutedEventArgs e)
        {
            //hay que hacer método de dejar juego en server y que el callback sea un ReceiveEndOfGame o algo así
        }

        private void BtnShowCards(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            OpenDialogDeck();
        }

        private void BtnRollDice(object sender, RoutedEventArgs e)
        {
           int rollDice = GameManager.RollDice();
            Console.WriteLine("Los dados son: ");
            Console.WriteLine(rollDice);

           OpenDialogRollDice(rollDice);
        }

        private void BtnAccuse(object sender, RoutedEventArgs e)
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

                default: //Llamar la opción de avisar que el movimiento es inválido
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
            //falta jeje
        }

        public void UpdateNumberOfPlayersInGameboard(int numberOfPlayers)
        {
            throw new NotImplementedException();
        }

        public void LeaveGameBoard()
        {
            GoToMainMenu();
        }
    }
}
