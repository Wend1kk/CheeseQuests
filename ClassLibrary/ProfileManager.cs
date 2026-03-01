using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
namespace GameLibrary
{
    public class ProfileManager
    {
        private static string ProfilePath(string username) => $"profiles/{username}.json";
        public static bool ProfileExists(string username) => File.Exists(ProfilePath(username));
        public static void SaveProfile(PlayerProfile profile)
        {
            Directory.CreateDirectory("profiles");
            string json = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ProfilePath(profile.Username), json);
        }
        public static PlayerProfile LoadProfile(string username)
        {
            if (ProfileExists(username))
            {
                string json = File.ReadAllText(ProfilePath(username));
                return JsonSerializer.Deserialize<PlayerProfile>(json);
            }
            return null;
        }
    }
}
