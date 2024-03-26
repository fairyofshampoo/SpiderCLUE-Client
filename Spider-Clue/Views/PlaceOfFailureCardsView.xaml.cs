using System.Windows;

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
            GetSinister("place1.png");
        }

        private void BtnComputerCenterCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place2.png");
        }

        private void BtnProfessorsRoomCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place3.png");
        }

        private void BtnFieldCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place4.png");
        }

        private void BtnParkingLotCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place5.png");
        }

        private void BtnF103Card_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place6.png");
        }

        private void BtnCubiclesCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place7.png");
        }

        private void BtnLaboratoryCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place8.png");
        }

        private void BtnAmphitheaterCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("place9.png");
        }

        private void GetSinister(string place)
        {
            PlaceCard = place;
            DialogResult = true;
            this.Close();
        }
    }
}
