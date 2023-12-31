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
    public partial class ReasonForFailureCardsView : Window
    {
        public string ReasonCard { get; set; }

        public ReasonForFailureCardsView()
        {
            InitializeComponent();
        }

        private void BtnMotive1_Click(object sender, RoutedEventArgs e)
        {
            GetMotive("motive1.png");
        }

        private void BtnMotive2_Click(object sender, RoutedEventArgs e)
        {
            GetMotive("motive2.png");
        }

        private void BtnMotive3_Click(object sender, RoutedEventArgs e)
        {
            GetMotive("motive3.png");
        }

        private void BtnMotive4_Click(object sender, RoutedEventArgs e)
        {
            GetMotive("motive4.png");
        }

        private void BtnMotive5_Click(object sender, RoutedEventArgs e)
        {
            GetMotive("motive5.png");
        }

        private void BtnMotive6_Click(object sender, RoutedEventArgs e)
        {
            GetMotive("motive6.png");
        }

        private void GetMotive(string motive)
        {
            ReasonCard = motive;
            DialogResult = true;
            this.Close();
        }
    }
}
