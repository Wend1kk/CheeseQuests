
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace Cheese_Quest
{
    public partial class MainWindow : Window
    {
        public static MediaPlayer GlobalmediaPlayer = new MediaPlayer();
        private const string RememberMeFile = "remember_me.txt";
        private PlayerProfile currentPlayer;
        public MainWindow()
        {
            InitializeComponent();
            CheckLogin();
            this.Closed += MainWindow_Closed;
        }
        public MainWindow(PlayerProfile playerProfile)
        {
            InitializeComponent();
            currentPlayer = playerProfile;
            UsernameTextBlock.Text = currentPlayer.Username;
            CheeseCountText.Text = currentPlayer.TotalCheeseCollected.ToString();
            StartMusic();
        }
        private void StartMusic()
        {
            GlobalmediaPlayer.Open(new Uri("Music.mp3", UriKind.Relative));
            GlobalmediaPlayer.Volume = 0.5;
            GlobalmediaPlayer.Play();
            GlobalmediaPlayer.MediaEnded += (sender, e) =>
            {
                GlobalmediaPlayer.Position = TimeSpan.Zero;
                GlobalmediaPlayer.Play();
            };
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            GlobalmediaPlayer.Stop(); 
            GlobalmediaPlayer.Close(); 
        }
        private void CheckLogin()
        {
            if (File.Exists(RememberMeFile))
            {
                string username = File.ReadAllText(RememberMeFile);
                if (ProfileManager.ProfileExists(username))
                {
                    currentPlayer = ProfileManager.LoadProfile(username);
                    OpenNewMainWindow();
                    UsernameTextBlock.Text = $"{currentPlayer.Username}";
                    CheeseCountText.Text = $"{currentPlayer.TotalCheeseCollected}";
                    StartMusic();
                    return;
                }
            }
            var registerWindow = new Register();
            bool? result = registerWindow.ShowDialog();
            if (result == true && registerWindow.RegisteredProfile != null)
            {
                currentPlayer = registerWindow.RegisteredProfile;
                OpenNewMainWindow();
                UsernameTextBlock.Text = $"{currentPlayer.Username}";
                CheeseCountText.Text = $"{currentPlayer.TotalCheeseCollected}";
                StartMusic();
                return;
            }
            var loginWindow = new Login();
            result = loginWindow.ShowDialog();
            if (result == true && loginWindow.LoggedInProfile != null)
            {
                currentPlayer = loginWindow.LoggedInProfile;
                OpenNewMainWindow();
                UsernameTextBlock.Text = $"{currentPlayer.Username}";
                CheeseCountText.Text = $"{currentPlayer.TotalCheeseCollected}";
                StartMusic();
                return;
            }
            Close();
        }
        private void OpenNewMainWindow()
        {
            MainWindow newMainWindow = new MainWindow(currentPlayer);
            newMainWindow.Show();
            this.Close(); 
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(GlobalmediaPlayer);
            settingsWindow.ShowDialog();
        }

        private void TipsButton_Click(object sender, RoutedEventArgs e)
        {
            TipsWindow tipsWindow = new TipsWindow();
            tipsWindow.ShowDialog();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            LevelSelectWindow levelSelectWindow = new LevelSelectWindow(currentPlayer);
            levelSelectWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string rememberFile = "remember_me.txt";
            if (File.Exists(rememberFile))
            {
                File.Delete(rememberFile);
            }
            GlobalmediaPlayer.Stop();
            GlobalmediaPlayer.Close();
            var registerWindow = new Register();
            registerWindow.Show();
            this.Close();
        }
    }
}
