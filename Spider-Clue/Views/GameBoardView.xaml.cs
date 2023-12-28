﻿using Spider_Clue.Logic;
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
            SetPawnsInBoard();

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

        public void GoToMainMenu()
        {
            //hay que checar la lógica de esto porque si no llegan todos los jugadores a conectarse, hay que terminar la partida
            //el inivtado debe ir al guest main menu
        }

        private void SetPawnsInBoard()
        {
            //en este método debería setear los pawn que recibió
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
            //hay que hacer método de dejar juego en server y que el callback sea un ReceiveEndOfGame o algo así
        }

        private void BtnShowCards(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            OpenDialogDeck();
        }

        private void BtnRollDice(object sender, RoutedEventArgs e)
        {
           GameManager.RollDice();
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
            //Si el pawn es diferente a nulo manda a llamar el método de mover
            //Si el pawn es nulo manda a llamar el método para avisar que no se puede

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

        public void ReceiveRollDice(int rollDice)
        {
            OpenDialogRollDice(rollDice);
        }
    }
}
