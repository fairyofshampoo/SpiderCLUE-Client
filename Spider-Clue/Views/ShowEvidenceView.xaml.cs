using Spider_Clue.Logic;
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
    public partial class ShowEvidenceView : Window
    {
        public ShowEvidenceView(string cardID)
        {
            InitializeComponent();
            ShowCard(cardID);
        }

        private void ShowCard(string cardID)
        {
            string cardPath = Utilities.GetImagePathForCards(cardID);
            this.DataContext = new { EvidenceCardPath = cardPath };
        }
    }
}
