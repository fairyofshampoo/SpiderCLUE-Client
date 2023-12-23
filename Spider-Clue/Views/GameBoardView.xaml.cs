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
    /// <summary>
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView : Page
    {
        public GameBoardView()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, MouseButtonEventArgs e)
        {
            Point click = e.GetPosition(GameBoardGrid);
            var columnClick = (int)(click.X / (int)GameBoardGrid.ActualWidth * GameBoardGrid.ColumnDefinitions.Count);
            var rowClick = (int)(click.Y / (int)GameBoardGrid.ActualHeight * GameBoardGrid.RowDefinitions.Count);

            Grid.SetRow(bluePawn, rowClick);
            Grid.SetColumn(bluePawn, columnClick);
        }
    }
}
