
using GameLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Level3 : Window
    {
        private MouseCharacter mouse;
        private CheeseManager cheeseManager;
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        private List<Rectangle> walls = new List<Rectangle>();
        private GameTimer levelTimer;
        private DateTime levelStartTime;
        private bool isLevelCompleted = false;
        private bool isLevelFailed = false;
        private CatCharacter cat;
        private PlayerProfile currentPlayer;
        public static MediaPlayer GlobalmediaPlayer = new MediaPlayer();
        public Level3(PlayerProfile player)
        {
            InitializeComponent();
            currentPlayer = player;
            GlobalmediaPlayer = MainWindow.GlobalmediaPlayer;
            cheeseManager = new CheeseManager(CheeseCountText, DoorImage, 14);
            cheeseManager.AddCheeseFromCanvas(MyCanvas, 14);
            mouse = new MouseCharacter(MouseImage, walls, cheeseManager);
            cat = new CatCharacter((Image)MyCanvas.FindName("Cat"), walls, path);
            cat.CatCaughtMouse += OnCatCaughtMouse;
            walls.Add(TopBorder);
            walls.Add(BottomBorder);
            walls.Add(LeftBorder);
            walls.Add(RightBorder);
            walls.Add(Shelf);
            walls.Add(Shelf2);
            walls.Add(WallLeft);
            walls.Add(WallRight);
            walls.Add(WallCenter);
            this.KeyDown += (s, e) => pressedKeys.Add(e.Key);
            this.KeyUp += (s, e) => pressedKeys.Remove(e.Key);
            CompositionTarget.Rendering += GameLoop;
            StartLevelTimer();
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
            cat.Update(mouse.MouseImage);
        }
        private void OnCatCaughtMouse()
        {
            mouse.TakeDamage();
            UpdateLivesUI(mouse.Lives); 
            if (mouse.Lives <= 0)
            {
                CompositionTarget.Rendering -= GameLoop;
                EndGameFailed();
            }
        }
        private void UpdateLivesUI(int lives)
        {
            Heart1.Visibility = (lives >= 1) ? Visibility.Visible : Visibility.Hidden;
            Heart2.Visibility = (lives >= 2) ? Visibility.Visible : Visibility.Hidden;
            Heart3.Visibility = (lives >= 3) ? Visibility.Visible : Visibility.Hidden;
        }
        private void StartLevelTimer()
        {
            levelStartTime = DateTime.Now;
            levelTimer = new GameTimer(2);
            levelTimer.TimeUpdated += (timeText) => TimerText.Text = timeText;
            levelTimer.TimeUp += EndGameFailed;
            levelTimer.Start();
        }
        private void EndGameComplete()
        {
            if (isLevelCompleted) return;
            isLevelCompleted = true;
            Level4 nextLevel = new Level4(currentPlayer);
            levelTimer.Stop();
            TimeSpan timeUsed = DateTime.Now - levelStartTime;
            string formattedTime = timeUsed.ToString(@"mm\:ss");
            int collectedCheese = cheeseManager.CollectedCheeseCount;
            LevelComplete completeWindow = new LevelComplete(formattedTime, this, currentPlayer, collectedCheese, this);
            completeWindow.Show();
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
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LevelSelectWindow levelMenu = new LevelSelectWindow(currentPlayer);
            levelMenu.Show();
            this.Close();
        }
        List<Point> path = new List<Point>
        {
            new Point(320, 97),
            new Point(320, 320),
            new Point(320, 98),
            new Point(600, 97)
        };
    }
}
