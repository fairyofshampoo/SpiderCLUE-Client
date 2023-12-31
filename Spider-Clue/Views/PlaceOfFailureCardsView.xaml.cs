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

    public partial class PlaceOfFailureCardsView : Window
    {
        public string PlaceCard { get; set; }
        public PlaceOfFailureCardsView()
        {
            InitializeComponent();
        }

        private void BtnGlassRoomCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place1");
        }

        private void BtnComputerCenterCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place2");
        }

        private void BtnProfessorsRoomCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place3");
        }

        private void BtnFieldCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place4");
        }

        private void BtnParkingLotCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place5");
        }

        private void BtnF103Card_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place6");
        }

        private void BtnCubiclesCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place7");
        }

        private void BtnLaboratoryCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place8");
        }

        private void BtnAmphitheaterCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place9");
        }

        private void GetSinister(string place)
        {
            PlaceCard = place;
            DialogResult = true;
            this.Close();
        }
    }
}
