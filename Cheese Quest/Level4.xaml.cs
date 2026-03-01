
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
    public partial class Level4 : Window
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
        private CatCharacter cat2;
        private PlayerProfile currentPlayer;
        public static MediaPlayer GlobalmediaPlayer = new MediaPlayer();
        public Level4(PlayerProfile player)
        {
            InitializeComponent();
            currentPlayer = player;
            GlobalmediaPlayer = MainWindow.GlobalmediaPlayer;
            cheeseManager = new CheeseManager(CheeseCountText, DoorImage, 15);
            cheeseManager.AddCheeseFromCanvas(MyCanvas, 15);
            mouse = new MouseCharacter(MouseImage, walls, cheeseManager);
            walls.Add(TopBorder);
            walls.Add(BottomBorder);
            walls.Add(LeftBorder);
            walls.Add(RightBorder);
            walls.Add(Shelf);
            walls.Add(Shelf2);
            walls.Add(WallCenter);
            walls.Add(WallCenter2);
            walls.Add(WallCenter3);
            walls.Add(WallCenter4);
            cat = new CatCharacter((Image)MyCanvas.FindName("Cat"), walls, catpath, false, 1);
            cat2 = new CatCharacter((Image)MyCanvas.FindName("Cat2"), walls, cat2path, true, 1);
            cat.CatCaughtMouse += OnCatCaughtMouse;
            cat2.CatCaughtMouse += OnCatCaughtMouse;
            this.KeyDown += (s, e) => pressedKeys.Add(e.Key);
            this.KeyUp += (s, e) => pressedKeys.Remove(e.Key);
            CompositionTarget.Rendering += GameLoop;
            StartLevel();
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
            cat2.Update(mouse.MouseImage);

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
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LevelSelectWindow levelMenu = new LevelSelectWindow(currentPlayer);
            levelMenu.Show();
            this.Close();
        }
        List<Point> catpath = new List<Point>
        {
            new Point(490, 83),
            new Point(630, 83),
            new Point(630, 300),
            new Point(630, 83),
            new Point(490, 83),
            new Point(630, 83),
        };
        List<Point> cat2path = new List<Point>
        {
           new Point(200, 66), 
           new Point(370, 66),
           new Point(200, 66),  
           new Point(200, 300),  
           new Point(200, 66),  
           new Point(370, 66), 
           new Point(200, 66)
        };
    }
}
