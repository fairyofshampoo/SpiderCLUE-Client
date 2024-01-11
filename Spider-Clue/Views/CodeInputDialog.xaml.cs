using System.Windows;

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
