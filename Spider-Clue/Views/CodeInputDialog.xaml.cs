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
    public partial class CodeInputDialog : Window
    {
        public string EmailValidation { get; set; }

        public CodeInputDialog()
        {
            InitializeComponent();
        }

        private void BtnVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            EmailValidation = txtCode.Text;
            DialogResult = true;
        }
    }
}
