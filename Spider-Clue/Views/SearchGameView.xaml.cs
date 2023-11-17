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
    /// <summary>
    /// Interaction logic for SearchGameView.xaml
    /// </summary>
    public partial class SearchGameView : Window
    {
        public SearchGameView()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, MouseButtonEventArgs e)
        {
            SearchMatch();
            ShowMatchFounded();
            ShowSearchNotFoundAlert();
        }

        private void SearchMatch()
        {

        }

        private void ShowMatchFounded()
        {

        }

        private void ShowSearchNotFoundAlert()
        {

        }

        private void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {
            JoinMatch();
        }

        private void JoinMatch()
        {

        }
    }
}
