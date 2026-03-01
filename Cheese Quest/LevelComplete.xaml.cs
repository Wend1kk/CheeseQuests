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
using GameLibrary;

namespace Cheese_Quest
{
    public partial class LevelComplete : Window
    {
        private Window currentLevel;
        private PlayerProfile currentPlayer;
        private int collectedCheese;
        private Window parentLevelWindow;
        public LevelComplete(string elapsedTime, Window currentLevel,PlayerProfile currentPlayer, int collectedCheese, Window parent)
        {
            InitializeComponent();
            TimeText.Text = "Time: " + elapsedTime;
            this.currentLevel = currentLevel;
            this.currentPlayer = currentPlayer;
            this.collectedCheese = collectedCheese;
            currentPlayer.TotalCheeseCollected += collectedCheese;
            ProfileManager.SaveProfile(currentPlayer);
            parentLevelWindow = parent;
        }
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            currentLevel.Close();
            if (currentLevel is Level1)
            {
                new Level2(currentPlayer).Show();
            }
            else if (currentLevel is Level2)
            {
                new Level3(currentPlayer).Show();
            }
            else if (currentLevel is Level3)
            {
                new Level4(currentPlayer).Show();
            }
            else if (currentLevel is Level4)
            {
                MessageBox.Show("Вітаю! Ви пройшли демо-версію гри!");
            }
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow menu = new MainWindow(currentPlayer);
            menu.Show();
            parentLevelWindow?.Close();
            this.Close();
        }
    }
}


