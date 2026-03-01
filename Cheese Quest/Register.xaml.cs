using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Cheese_Quest
{

    public partial class Register : Window
    {
        public PlayerProfile RegisteredProfile { get; private set; }
        public Register()
        {
            InitializeComponent();
        }
        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            if (!IsValidUsername(username))
            {
                MessageBox.Show("Ім'я користувача має містити від 3 до 15 символів: літери, цифри або підкреслення.");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введіть коректну електронну адресу.");
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Пароль повинен містити мінімум 6 символів, включно з хоча б однією літерою та цифрою.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не співпадають.");
                return;
            }

            if (ProfileManager.ProfileExists(username))
            {
                MessageBox.Show("Користувач з таким ім’ям вже існує.");
                return;
            }

            var profile = new PlayerProfile
            {
                Username = username,
                Email = email,
                Password = password
            };
            ProfileManager.SaveProfile(profile);
            MessageBox.Show("Реєстрація пройшла успішно!");
            RegisteredProfile = profile;
            DialogResult = true;
            Close();
        }
        private bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, @"^[a-zA-Z0-9_]{3,15}$");
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
        }
    }
}


