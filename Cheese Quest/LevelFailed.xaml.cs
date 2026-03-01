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
    public partial class LevelFailed : Window
    {
        private Window currentLevel;
        private PlayerProfile currentPlayer;
        private int collectedCheese;
        public LevelFailed(string elapsedTime, Window currentLevel, PlayerProfile currentPlayer, int collectedCheese)
        {
            InitializeComponent();
            TimeText.Text = "Time: " + elapsedTime;
            this.currentLevel = currentLevel;
            this.currentPlayer = currentPlayer;
            this.collectedCheese = collectedCheese;
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow menu = new MainWindow(currentPlayer);
            menu.Show();
            this.Close();
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Type levelType = currentLevel.GetType();
            object[] constructorArgs = new object[] { currentPlayer };
            Window newLevel = (Window)Activator.CreateInstance(levelType, constructorArgs);
            newLevel.Show();
            this.Close();
            currentLevel.Close();
        }
    }
}
