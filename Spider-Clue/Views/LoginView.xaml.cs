﻿using System;
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
 
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LblRegister_Clicked(object sender, MouseButtonEventArgs mouseEvent)
        {
            Register registerView = new Register();
            this.NavigationService.Navigate(registerView);
        }

        private void BtnGuestPlayer_Click(object sender, RoutedEventArgs e)
        {
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }

        private void LblForgotPassword_Clicked(object sender, MouseButtonEventArgs e)
        {
            AccountRecoveryView accountRecoveryView = new AccountRecoveryView();
            this.NavigationService.Navigate(accountRecoveryView);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
