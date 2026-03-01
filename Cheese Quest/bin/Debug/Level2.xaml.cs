
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
    public partial class Level2 : Window
    {
        private MouseCharacter mouse;
        private CheeseManager cheeseManager;
        private HashSet<Key> pressedKeys = new HashSet<Key>();
        private List<Rectangle> walls = new List<Rectangle>();
        private Image hole1;
        private Image hole2;
        private int teleportCooldown = 0;
        private List<Image> traps = new List<Image>();
        private GameTimer levelTimer;
        private DateTime levelStartTime;
        private bool isLevelCompleted = false;
        private bool isLevelFailed = false;
        private PlayerProfile currentPlayer;
        public static MediaPlayer GlobalmediaPlayer = new MediaPlayer();
        public Level2(PlayerProfile player)
        {
            InitializeComponent();
            currentPlayer = player;
            GlobalmediaPlayer = MainWindow.GlobalmediaPlayer;
            hole1 = (Image)MyCanvas.FindName("Hole1");
            hole2 = (Image)MyCanvas.FindName("Hole2");
            cheeseManager = new CheeseManager(CheeseCountText, DoorImage, 14);
            cheeseManager.AddCheeseFromCanvas(MyCanvas, 14);
            AddTrapsFromCanvas(MyCanvas);
            mouse = new MouseCharacter(MouseImage, walls, cheeseManager);
            walls.Add(TopBorder);
            walls.Add(BottomBorder);
            walls.Add(LeftBorder);
            walls.Add(RightBorder);
            walls.Add(Shelf);
            walls.Add(Shelf2);
            walls.Add(WallLeft);
            walls.Add(WallCenter);
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
            if (teleportCooldown > 0)
            {
                teleportCooldown--;
            }
            CheckTeleport(mouse);
            CheckTrapCollision(MouseImage);
        }
        private void StartLevel()
        {
            levelStartTime = DateTime.Now;
            levelTimer = new GameTimer(2);
            levelTimer.TimeUpdated += UpdateTimerUI;
            levelTimer.Start();
        }
        private void UpdateTimerUI(string timeText)
        {
            TimerText.Text = timeText;
        }
        private void EndGameComplete()
        {
            if (isLevelCompleted) return;
            isLevelCompleted = true;
            Level3 nextLevel = new Level3(currentPlayer);
            levelTimer.Stop();
            TimeSpan timeUsed = DateTime.Now - levelStartTime;
            string formattedTime = timeUsed.ToString(@"mm\:ss");
            int collectedCheese = cheeseManager.CollectedCheeseCount;
            LevelComplete completeWindow = new LevelComplete(formattedTime, this, currentPlayer, collectedCheese, this);
            completeWindow.Show();
        }

        private void EndGameFailed()
        {
            if(isLevelFailed) return;
            isLevelFailed = true;
            levelTimer.Stop();
            TimeSpan timeUsed = DateTime.Now - levelStartTime;
            string formattedTime = timeUsed.ToString(@"mm\:ss");
            int collectedCheeseCount = cheeseManager.CollectedCheeseCount;
            LevelFailed failedWindow = new LevelFailed(formattedTime, this, currentPlayer, collectedCheeseCount);
            failedWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LevelSelectWindow levelMenu = new LevelSelectWindow(currentPlayer);
            levelMenu.Show();
            this.Close();
        }
        private void CheckTeleport(MouseCharacter mouse)
        {
            if (teleportCooldown > 0) return;
            Rect hole1Rect = new Rect(Canvas.GetLeft(hole1), Canvas.GetTop(hole1), hole1.Width, hole1.Height);
            Rect hole2Rect = new Rect(Canvas.GetLeft(hole2), Canvas.GetTop(hole2), hole2.Width, hole2.Height);
            if (mouse.Bounds.IntersectsWith(hole1Rect))
            {
                TeleportMouse(mouse, hole2);
                teleportCooldown = 60;
            }
            else if (mouse.Bounds.IntersectsWith(hole2Rect))
            {
                TeleportMouse(mouse, hole1);
                teleportCooldown = 60;
            }
        }
        private void TeleportMouse(MouseCharacter mouse, Image targetHole)
        {
            Canvas.SetLeft(mouse.MouseImage, Canvas.GetLeft(targetHole));
            Canvas.SetTop(mouse.MouseImage, Canvas.GetTop(targetHole));
        }
        private void AddTrapsFromCanvas(Canvas canvas)
        {
            foreach (UIElement element in canvas.Children)
            {
                if (element is Image image && image.Name.StartsWith("Trap"))
                {
                    traps.Add(image);
                }
            }
        }
        private void UpdateLivesDisplay(int lives)
        {
            Heart1.Visibility = lives >= 1 ? Visibility.Visible : Visibility.Hidden;
            Heart2.Visibility = lives >= 2 ? Visibility.Visible : Visibility.Hidden;
            Heart3.Visibility = lives >= 3 ? Visibility.Visible : Visibility.Hidden;
        }
        public void CheckTrapCollision(Image MouseImage)
        {
            double mouseLeft = Canvas.GetLeft(MouseImage);
            double mouseTop = Canvas.GetTop(MouseImage);
            foreach (Image trap in traps)
            {
                if (trap.Visibility == Visibility.Visible) 
                {
                    double trapLeft = Canvas.GetLeft(trap);
                    double trapTop = Canvas.GetTop(trap);

                    if (mouseLeft + MouseImage.Width > trapLeft &&
                        mouseLeft < trapLeft + trap.Width &&
                        mouseTop + MouseImage.Height > trapTop &&
                        mouseTop < trapTop + trap.Height)
                    {
                        mouse.TakeDamage();
                        UpdateLivesDisplay(mouse.Lives);
                        if (mouse.Lives <= 0)
                        {
                            EndGameFailed();
                        }
                        return;
                    }
                }

            }
           
        }
    }
}

