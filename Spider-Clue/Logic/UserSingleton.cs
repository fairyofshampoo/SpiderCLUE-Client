using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider_Clue.Logic
{
    internal class UserSingleton
    {
        private static readonly UserSingleton _instance = new UserSingleton();
        public string GamerTag { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Level { get; set; }

        private UserSingleton() { }

        public void Initialize(string gamerTag, string name, string lastName, string email)
        {
            GamerTag = gamerTag;
            Name = name;
            LastName = lastName;
            Email = email;
        }

        public void Clear()
        {
            GamerTag = null;
            Name = null;
            LastName = null;
            Email = null;
        }


        public static UserSingleton Instance => _instance;
    }
    
}
