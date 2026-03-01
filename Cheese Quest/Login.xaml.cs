using GameLibrary;
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
using System.IO;
namespace Cheese_Quest
{
    public partial class Login : Window
    {
        public PlayerProfile LoggedInProfile { get; private set; }
        private const string RememberMeFile = "remember_me.txt";
        public Login()
        {
            InitializeComponent();
            LoadRememberedUser();
        }
        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.Show();
            this.Close();
        }
        private void LoadRememberedUser()
        {
            if (File.Exists(RememberMeFile))
            {
                string rememberedUsername = File.ReadAllText(RememberMeFile);
                UsernameTextBox.Text = rememberedUsername;
                RememberMeCheckBox.IsChecked = true;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Будь ласка, введіть ім’я користувача та пароль.");
                return;
            }
            if (!ProfileManager.ProfileExists(username))
            {
                MessageBox.Show("Користувача з таким ім’ям не знайдено.");
                return;
            }
            var profile = ProfileManager.LoadProfile(username);
            if (profile.Password != password)
            {
                MessageBox.Show("Неправильний пароль.");
                return;
            }
            if (RememberMeCheckBox.IsChecked == true)
            {
                File.WriteAllText(RememberMeFile, username);
            }
            else if (File.Exists(RememberMeFile))
            {
                File.Delete(RememberMeFile);
            }
            LoggedInProfile = profile;
            DialogResult = true;
            Close();
        }
    }
}
