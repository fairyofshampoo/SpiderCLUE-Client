﻿using Spider_Clue.Logic;
using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

    public partial class FriendsListView : Page
    {
        public string[] FriendsConnected { get; set; }

        public FriendsListView(String[] friendsConnected)
        {
            InitializeComponent();
            FriendsConnected = friendsConnected;
            ShowFriendList();
        }

        private void ShowFriendList()
        {
            string[] friendList = GetFriends();
            string statusColor = "Red";
            for(int firstIndex = 0; firstIndex < friendList.Length; firstIndex++)
            {
                for (int secondIndex = 0; secondIndex < FriendsConnected.Length; secondIndex++)
                {
                    if (friendList[firstIndex] == FriendsConnected[secondIndex])
                    {
                        statusColor = "Green";
                    }
                }
                Player player = new Player
                {
                    Gamertag = friendList[firstIndex],
                    Status = statusColor
                };
                FriendsConnectedGrid.Items.Add(player);
                statusColor = "Red";
            }           
        }

        private string [] GetFriends()
        {
            SpiderClueService.IFriendshipManager friendRequest = new SpiderClueService.FriendshipManagerClient();
            string [] friendList = friendRequest.GetFriendList(UserSingleton.Instance.GamerTag);
            return friendList;
        }

        public class Player
        {
            public string Gamertag { get; set; }
            public string Status { get; set; }
        }

        private void BtnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            OpenDialogSendFriendRequest();
        }

        private void OpenDialogSendFriendRequest()
        {
            Window mainWindow = Window.GetWindow(this);
            SendFriendRequest sendFriendRequest = new SendFriendRequest();
            sendFriendRequest.Owner = mainWindow;
            sendFriendRequest.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            sendFriendRequest.ShowDialog();
        }

        private void BtnChangeFriendRequest(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            FriendsRequestView friendRequestView = new FriendsRequestView();
            NavigationService.Navigate(friendRequestView);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            MainMenuView mainMenuView = new MainMenuView();
            this.NavigationService.Navigate(mainMenuView);
        }
    }
}
