using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
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
    public partial class SinisterSixCardsView : Window
    {
        public string SinisterCard { get; set; }
        public SinisterSixCardsView()
        {
            InitializeComponent();
        }

        private void BtnDocOchCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("DocOchCard.png");
        }

        private void BtnMysteRevoCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("MysteRevoCard.png");
        }

        private void BtnRhinosidroCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("RhinosidroCard.png");
        }

        private void BtnElectroJuanCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("ElectroJuanCard.png");
        }

        private void BtnXandManCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("XandManCard.png");
        }

        private void BtnVernomCard_Click(object sender, RoutedEventArgs e)
        {
            GetSinister("VernomCard.png");
        }

        private void GetSinister(string sinister)
        {
            SinisterCard = sinister;
            DialogResult = true;
            this.Close();
        }
    }
}
