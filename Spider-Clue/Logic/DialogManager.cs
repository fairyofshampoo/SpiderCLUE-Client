using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spider_Clue.Logic
{
    public static class DialogManager
    {
        public static void ShowErrorMessageBox(string errorMessage)
        {
            MessageBox.Show(errorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowWarningMessageBox(string warningMessage)
        {
            MessageBox.Show(warningMessage, Properties.Resources.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowSuccessMessageBox(string successMessage)
        {
            MessageBox.Show(successMessage, Properties.Resources.SuccessTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
