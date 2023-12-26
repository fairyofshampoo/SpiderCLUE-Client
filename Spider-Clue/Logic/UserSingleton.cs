using Spider_Clue.SpiderClueService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spider_Clue.Logic
{
    internal class UserSingleton
    {
        private static readonly UserSingleton _instance = new UserSingleton();

        public String GamerTag { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int GamesWon { get; set; }
        public string ImageCode { get; set; }
        public bool IsGuestPlayer { get; set; }

        private UserSingleton() { }

        public void Initialize(Gamer gamer)
        {
            GamerTag = gamer.Gamertag;
            Name = gamer.FirstName;
            LastName = gamer.LastName;
            Email = gamer.Email;
            GamesWon = gamer.GamesWon;
            ImageCode = gamer.ImageCode;
            IsGuestPlayer = false;
        }

        public void Clear()
        {
            GamerTag = null;
            Name = null;
            LastName = null;
            Email = null;
            ImageCode = "Icon0.png";
        }


        public static UserSingleton Instance => _instance;
    }
    
}
