
using System.Windows;

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
