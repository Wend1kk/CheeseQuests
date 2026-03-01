
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

namespace Cheese_Quest
{
    public partial class Level1 : Window
    {
        private MouseCharacter mouse;
        private List<Rectangle> walls = new List<Rectangle>();
        private CheeseManager cheeseManager;
        private GameTimer levelTimer;
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        private bool isLevelCompleted = false;
        private DateTime levelStartTime;
        private bool isLevelFailed = false;
        private PlayerProfile currentPlayer;
        public static MediaPlayer GlobalmediaPlayer = new MediaPlayer();
        public Level1(PlayerProfile player)
        {
            InitializeComponent();
            currentPlayer = player;
            GlobalmediaPlayer = MainWindow.GlobalmediaPlayer;
            StartLevel();
            cheeseManager = new CheeseManager(CheeseCountText, DoorImage, 16);
            cheeseManager.AddCheeseFromCanvas(MyCanvas, 16);
            mouse = new MouseCharacter(MouseImage, walls, cheeseManager);
            walls.Add(TopBorder);
            walls.Add(BottomBorder);
            walls.Add(LeftBorder);
            walls.Add(RightBorder);
            walls.Add(Table);
            walls.Add(Shelf);
            walls.Add(Shelf2);
            walls.Add(WallLeft);
            walls.Add(WallRight);
            walls.Add(WallCenter);
            this.KeyDown += (s, e) => pressedKeys.Add(e.Key);
            this.KeyUp += (s, e) => pressedKeys.Remove(e.Key);
            CompositionTarget.Rendering += GameLoop;
        }
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(GlobalmediaPlayer);
            settingsWindow.ShowDialog();
        }
        private void GameLoop(object sender, EventArgs e)
        {
            mouse.HandleMovement(pressedKeys);
            cheeseManager.CheckCheeseCollection(MouseImage);
            mouse.CheckDoorCollision(DoorImage, EndGameComplete);
        }
        private void StartLevel()
        {
            levelStartTime = DateTime.Now;
            levelTimer = new GameTimer(2);
            levelTimer.TimeUpdated += UpdateTimerUI;
            levelTimer.TimeUp += EndGameFailed;
            levelTimer.Start();
        }
        private void UpdateTimerUI(string timeText)
        {
            TimerText.Text = timeText;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LevelSelectWindow levelMenu = new LevelSelectWindow(currentPlayer); 
            levelMenu.Show();
            this.Close(); 
        }
        private void EndGameComplete()
        {
            if (isLevelCompleted) return;
            isLevelCompleted = true;
            Level2 nextLevel = new Level2(currentPlayer);
            levelTimer.Stop();
            TimeSpan timeUsed = DateTime.Now - levelStartTime;
            string formattedTime = timeUsed.ToString(@"mm\:ss");
            int collectedCheese = cheeseManager.CollectedCheeseCount;
            LevelComplete completeWindow = new LevelComplete(formattedTime, this, currentPlayer, collectedCheese, this);
            completeWindow.ShowDialog();
        }
        private void EndGameFailed()
        {
            if (isLevelFailed) return;
            isLevelFailed = true;
            levelTimer.Stop();
            TimeSpan timeUsed = DateTime.Now - levelStartTime;
            string formattedTime = timeUsed.ToString(@"mm\:ss");
            int collectedCheeseCount = cheeseManager.CollectedCheeseCount;
            LevelFailed failedWindow = new LevelFailed(formattedTime, this, currentPlayer, collectedCheeseCount);
            failedWindow.Show();
            this.Close();
        }
    }
}
