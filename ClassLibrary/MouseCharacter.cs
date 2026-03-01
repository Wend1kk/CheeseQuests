using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GameLibrary
{
    public class MouseCharacter
    {
        public Image MouseImage { get; set; }
        private CheeseManager cheeseManager;
        private List<Rectangle> walls;
        private double startLeft;
        private double startTop;
        public int Lives { get; private set; } = 3;
        public event Action OnMouseDead;
        public MouseCharacter(Image image, List<Rectangle> levelWalls, CheeseManager cheeseMgr)
        {
            MouseImage = image;
            startLeft = Canvas.GetLeft(image);
            startTop = Canvas.GetTop(image);
            walls = levelWalls;
            cheeseManager = cheeseMgr;
        }
        private void UpdateSprite(Key key)
        {
            if (mouseImages.ContainsKey(key))
            {
            MouseImage.Source = mouseImages[key];
            }
        }
        public void TakeDamage()
        {
            Lives--;
            if (Lives <= 0)
            {
                OnMouseDead?.Invoke();
            }
            else
            {
                ResetPosition();
            }
        }
        public void ResetPosition()
        {
            Canvas.SetLeft(MouseImage, startLeft);
            Canvas.SetTop(MouseImage, startTop);
        }
        private bool CheckIntersection(FrameworkElement obj1, FrameworkElement obj2)
        {
        Rect rect1 = new Rect(Canvas.GetLeft(obj1), Canvas.GetTop(obj1), obj1.Width, obj1.Height);
        Rect rect2 = new Rect(Canvas.GetLeft(obj2), Canvas.GetTop(obj2), obj2.Width, obj2.Height);
        return rect1.IntersectsWith(rect2);
        }
        private bool IsColliding(FrameworkElement obstacle)
        {
           return CheckIntersection(MouseImage, obstacle);
        }

        private Dictionary<Key, BitmapImage> mouseImages = new Dictionary<Key, BitmapImage>
        {
          { Key.Left, new BitmapImage(new Uri("Images/mouse_left.png", UriKind.Relative)) },
          { Key.Right, new BitmapImage(new Uri("Images/mouse.png", UriKind.Relative)) },
          { Key.Up, new BitmapImage(new Uri("Images/mouse_up.png", UriKind.Relative)) },
          { Key.Down, new BitmapImage(new Uri("Images/mouse_down.png", UriKind.Relative)) }
        };
        public void HandleMovement(HashSet<Key> pressedKeys)
        {
            double deltaX = 0;
            double deltaY = 0;
            if (pressedKeys.Contains(Key.Left))
            {
                deltaX = -3;
                UpdateSprite(Key.Left);
            }
            if (pressedKeys.Contains(Key.Right))
            {
                deltaX = 3;
                UpdateSprite(Key.Right);
            }
            if (pressedKeys.Contains(Key.Up))
            {
                deltaY = -3;
                UpdateSprite(Key.Up);
            }
            if (pressedKeys.Contains(Key.Down))
            {
                deltaY = 3;
                UpdateSprite(Key.Down);
            }
            Move(deltaX, deltaY);
        }
        public void Move(double deltaX, double deltaY)
        {
            double newLeft = Canvas.GetLeft(MouseImage) + deltaX;
            double newTop = Canvas.GetTop(MouseImage) + deltaY;
            if (!IsColliding(newLeft, newTop))
            {
                Canvas.SetLeft(MouseImage, newLeft);
                Canvas.SetTop(MouseImage, newTop);
            }
        }
        public void CheckDoorCollision(FrameworkElement door)
        {
            return CheckIntersection(MouseImage, door);
        }
        public Rect Bounds
        {
            get
            {
                double left = Canvas.GetLeft(MouseImage);
                double top = Canvas.GetTop(MouseImage);
                return new Rect(left, top, MouseImage.Width, MouseImage.Height);
            }
        }

    }
}
