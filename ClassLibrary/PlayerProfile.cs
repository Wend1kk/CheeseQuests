using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class PlayerProfile
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CurrentLevel { get; set; } = 1;
        public int TotalCheeseCollected { get; set; } = 0;
    }
}
