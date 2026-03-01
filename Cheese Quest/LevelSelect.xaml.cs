using Cheese_Quest;
using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace Cheese_Quest
{
    public partial class LevelSelectWindow : Window
    {
        private PlayerProfile currentPlayer;
        public LevelSelectWindow(PlayerProfile player)
        {
            InitializeComponent();
            currentPlayer = player;
        }
        private void Level1Button_Click(object sender, RoutedEventArgs e)
        {
            Level1 levelWindow = new Level1(currentPlayer);
            levelWindow.Show();
            this.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow levelMenu = new MainWindow(currentPlayer);
            levelMenu.Show();
            this.Close();
        }

        private void Level2Button_Click(object sender, RoutedEventArgs e)
        {
            Level2 level2Window = new Level2(currentPlayer);
            level2Window.Show();
            this.Close();
        }

        private void Level3_Click(object sender, RoutedEventArgs e)
        {
            Level3 level3Window = new Level3(currentPlayer);
            level3Window.Show();
            this.Close();
        }

        private void Level4_Click(object sender, RoutedEventArgs e)
        {
            Level4 level4Window = new Level4(currentPlayer);
            level4Window.Show();
            this.Close();
        }
    }
}
