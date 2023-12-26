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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spider_Clue.Views
{
    public partial class GameBoardView : Page , IGameManagerCallback
    {
        public GameBoardView()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, MouseButtonEventArgs mouseEvent)
        {
            Point click = mouseEvent.GetPosition(GameBoardGrid);
            int columnClick = (int)(click.X / (int)GameBoardGrid.ActualWidth * GameBoardGrid.ColumnDefinitions.Count);
            int rowClick = (int)(click.Y / (int)GameBoardGrid.ActualHeight * GameBoardGrid.RowDefinitions.Count);

            Grid.SetRow(bluePawn, rowClick);
            Grid.SetColumn(bluePawn, columnClick);

            
        }

        private void BtnLeaveGame(object sender, RoutedEventArgs e)
        {

        }

        private void BtnShowCards(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            OpenDialogDeck();
        }

        private void BtnRollDice(object sender, RoutedEventArgs e)
        {
            //Manda a llamar al que tira los dados y le notifica cuanto sacó 
        }

        private void BtnAccuse(object sender, RoutedEventArgs e)
        {
            //Realizar la acusación final 
            //Se tendrá que verificar cuál manda y ver si en el array es el mismo
            //Si sí es terminar la partida y guardar al usuario como ganador
            //Si no, deshabilia las funciones principales
        }

        public void ReceivePawnsMove()
        {
            throw new NotImplementedException();
        }

        public void ReceiveRollDice(int diceRoll)
        {
            OpenDialogRollDice(diceRoll);
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
    }
}
