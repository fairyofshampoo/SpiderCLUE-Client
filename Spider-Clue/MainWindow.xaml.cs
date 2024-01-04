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
            MessageBoxResult result = MessageBox.Show("¿Desea cerrar la app?", "Confirmar cierre", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
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

                if (UserSingleton.Instance.GamerTag != null)
                {
                    SpiderClueService.IUserManager userManager = new SpiderClueService.UserManagerClient();
                    if (UserSingleton.Instance.IsGuestPlayer)
                    {
                        try
                        {
                            userManager.DeleteGuestPlayer(UserSingleton.Instance.GamerTag);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error en Disconnect: {ex.Message}");
                        }
                    }
                    else
                    {
                        try
                        {
                            SpiderClueService.ISessionManager sessionManager = new SpiderClueService.SessionManagerClient();
                            sessionManager.Disconnect(UserSingleton.Instance.GamerTag);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error en Disconnect: {ex.Message}");
                        }
                    }
                }

                UserSingleton.Instance.Clear();
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