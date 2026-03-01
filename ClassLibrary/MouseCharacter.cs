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
        private double _moveSpeed = 3;
        public MouseCharacter(Image image, List<Rectangle> levelWalls, CheeseManager cheeseMgr)
        {
            MouseImage = image;
            startLeft = Canvas.GetLeft(image);
            startTop = Canvas.GetTop(image);
            walls = levelWalls;
            cheeseManager = cheeseMgr;
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
        private bool IsColliding(double newLeft, double newTop)
        {
            foreach (var wall in walls)
            {
                double wallLeft = Canvas.GetLeft(wall);
                double wallTop = Canvas.GetTop(wall);

                if (newLeft + MouseImage.Width > wallLeft &&
                    newLeft < wallLeft + wall.Width &&
                    newTop + MouseImage.Height > wallTop &&
                    newTop < wallTop + wall.Height)
                {
                    return true;
                }
            }
            return false;
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
                deltaX = -_moveSpeed;
                MouseImage.Source = mouseImages[Key.Left];
            }
            if (pressedKeys.Contains(Key.Right))
            {
                deltaX = _moveSpeed;
                MouseImage.Source = mouseImages[Key.Right];
            }
            if (pressedKeys.Contains(Key.Up))
            {
                deltaY = -_moveSpeed;
                MouseImage.Source = mouseImages[Key.Up];
            }
            if (pressedKeys.Contains(Key.Down))
            {
                deltaY = _moveSpeed;
                MouseImage.Source = mouseImages[Key.Down];
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
        public void CheckDoorCollision(Image doorImage, Action onLevelComplete)
        {
            if (doorImage.Source.ToString().Contains("Images/OpenDoor.png"))
            {
                double mouseLeft = Canvas.GetLeft(MouseImage);
                double mouseTop = Canvas.GetTop(MouseImage);

                double doorLeft = Canvas.GetLeft(doorImage);
                double doorTop = Canvas.GetTop(doorImage);

                if (mouseLeft + MouseImage.Width > doorLeft &&
                    mouseLeft < doorLeft + doorImage.Width &&
                    mouseTop + MouseImage.Height > doorTop &&
                    mouseTop < doorTop + doorImage.Height)
                {
                    onLevelComplete();
                }
            }
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
