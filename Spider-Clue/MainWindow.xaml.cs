using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using Spider_Clue.Views;
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

namespace Spider_Clue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = ShowConfirmationDialog("¿Desea cerrar la app?", "Confirmar cierre");

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                HandleNavigation();
                HandleUserDisconnect();
                UserSingleton.Instance.Clear();
            }
        }

        private MessageBoxResult ShowConfirmationDialog(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private void HandleNavigation()
        {
            if (NavigationFrame.Content is Page currentPage)
            {
                if (currentPage is LobbyView lobby)
                {
                    lobby.GoToMainMenu();
                }

                if (currentPage is GameBoardView gameBoard)
                {
                    gameBoard.LeaveGame();
                }
            }
        }

        private void HandleUserDisconnect()
        {
            if (UserSingleton.Instance.GamerTag != null)
            {
                SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();

                try
                {
                    if (UserSingleton.Instance.IsGuestPlayer)
                    {
                        userManager.DeleteGuestPlayer(UserSingleton.Instance.GamerTag);
                    }
                    else
                    {
                        SpiderClueService.ISessionManager sessionManager = new SpiderClueService.SessionManagerClient();
                        sessionManager.Disconnect(UserSingleton.Instance.GamerTag);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en Disconnect: {ex.Message}");
                }
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowStyle = WindowStyle.None;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
    }
}