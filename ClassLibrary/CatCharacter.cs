using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameLibrary
{
    public class CatCharacter
    {
        public Image Cat { get; private set; }
        private List<Rectangle> walls;
        private List<Point> pathPoints;
        private int currentPointIndex = 0;
        private bool chasingMouse = false;
        private bool movingForward = true;
        public double Speed { get; set; } 
        public event Action CatCaughtMouse;
        private Vector lastMoveDirection = new Vector(1, 0);
        private bool useSecondCatImages = false;
        public CatCharacter(Image image, List<Rectangle> levelWalls, List<Point> path, bool useSecondCatImages = false, double speed = 1.5)
        {
            Cat = image;
            walls = levelWalls;
            pathPoints = path;
            this.useSecondCatImages = useSecondCatImages;
            this.Speed = speed;
        }
        public void Update(Image mouse)
        {
            double catX = Canvas.GetLeft(Cat);
            double catY = Canvas.GetTop(Cat);
            double mouseX = Canvas.GetLeft(mouse);
            double mouseY = Canvas.GetTop(mouse);
            Vector lookDirection = lastMoveDirection;
            Vector toMouse = new Vector(mouseX - catX, mouseY - catY);
            toMouse.Normalize();
            double angle = Vector.AngleBetween(lookDirection, toMouse);
            bool inSightAngle = Math.Abs(angle) < 45;
            bool hasLineOfSight = HasLineOfSight(catX, catY, mouseX, mouseY);
            chasingMouse = inSightAngle && hasLineOfSight;
            if (IsTouchingMouse(mouse))
            {
                CatCaughtMouse?.Invoke();
                return;
            }
            if (chasingMouse)
            {
                MoveTowards(mouseX, mouseY);
            }
            else
            {
                FollowPath();
            }
        }
        private bool IsTouchingMouse(Image mouse)
        {
            Rect catRect = new Rect(Canvas.GetLeft(Cat), Canvas.GetTop(Cat), Cat.ActualWidth, Cat.ActualHeight);
            Rect mouseRect = new Rect(Canvas.GetLeft(mouse), Canvas.GetTop(mouse), mouse.ActualWidth, mouse.ActualHeight);
            return catRect.IntersectsWith(mouseRect);
        }
        private bool HasLineOfSight(double x1, double y1, double x2, double y2)
        {
            Rect lineRect = new Rect(Math.Min(x1, x2), Math.Min(y1, y2), Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            foreach (var wall in walls)
            {
                Rect wallRect = new Rect(Canvas.GetLeft(wall), Canvas.GetTop(wall), wall.Width, wall.Height);
                if (lineRect.IntersectsWith(wallRect))
                    return false;
            }
            return true;
        }
        private void FollowPath()
        {
            if (pathPoints == null || pathPoints.Count == 0)
            {
                return;
            }
            Point target = pathPoints[currentPointIndex];
            double catX = Canvas.GetLeft(Cat);
            double catY = Canvas.GetTop(Cat);

            if (Distance(catX, catY, target.X, target.Y) < 5)
            {
                if (movingForward)
                {
                    currentPointIndex++;
                    if (currentPointIndex >= pathPoints.Count)
                    {
                        currentPointIndex = pathPoints.Count - 1; 
                        movingForward = false;
                    }
                }
                else
                {
                    currentPointIndex--;
                    if (currentPointIndex < 0)
                    {
                        currentPointIndex = 0; 
                        movingForward = true;
                    }
                }
                target = pathPoints[currentPointIndex];
            }

            MoveTowards(target.X, target.Y);
        }
        private void MoveTowards(double targetX, double targetY)
        {
            double catX = Canvas.GetLeft(Cat);
            double catY = Canvas.GetTop(Cat);
            double dx = targetX - catX;
            double dy = targetY - catY;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            if (dist == 0) return;
            double deltaX = dx / dist * Speed;
            double deltaY = dy / dist * Speed;
            bool canMoveX = !IsColliding(catX + deltaX, catY);
            bool canMoveY = !IsColliding(catX, catY + deltaY);

            if (canMoveX && canMoveY)
            {
                Canvas.SetLeft(Cat, catX + deltaX);
                Canvas.SetTop(Cat, catY + deltaY);
            }
            else if (canMoveX)
            {
                Canvas.SetLeft(Cat, catX + deltaX);
            }
            else if (canMoveY)
            {
                Canvas.SetTop(Cat, catY + deltaY);
            }
            else
            {
                movingForward = !movingForward;
                currentPointIndex = movingForward ? 0 : pathPoints.Count - 1;
            }
            Vector moveVector = new Vector(deltaX, deltaY);
            if (moveVector.Length > 0)
            {
                moveVector.Normalize();
                lastMoveDirection = moveVector;
            }
            var imagesDict = useSecondCatImages ? cat2Images : catImages;
            Cat.Source = Math.Abs(deltaX) > Math.Abs(deltaY) ? (deltaX > 0 ? imagesDict["right"] : imagesDict["left"]) : (deltaY > 0 ? imagesDict["down"] : imagesDict["up"]);
        }
        private Dictionary<string, BitmapImage> catImages = new Dictionary<string, BitmapImage>
        {
           { "left", new BitmapImage(new Uri("Images/Cat_left.png", UriKind.Relative)) },
           { "right", new BitmapImage(new Uri("Images/Cat_right.png", UriKind.Relative)) },
           { "up", new BitmapImage(new Uri("Images/Cat_up.png", UriKind.Relative)) },
           { "down", new BitmapImage(new Uri("Images/Cat.png", UriKind.Relative)) }
        };
        private Dictionary<string, BitmapImage> cat2Images = new Dictionary<string, BitmapImage>
        {
           { "left", new BitmapImage(new Uri("Images/Cat2_left.png", UriKind.Relative)) },
           { "right", new BitmapImage(new Uri("Images/Cat2_right.png", UriKind.Relative)) },
           { "up", new BitmapImage(new Uri("Images/Cat2_up.png", UriKind.Relative)) },
           { "down", new BitmapImage(new Uri("Images/Cat2.png", UriKind.Relative)) }
        };
        private bool IsColliding(double newLeft, double newTop)
        {
            foreach (var wall in walls)
            {
                double wallLeft = Canvas.GetLeft(wall);
                double wallTop = Canvas.GetTop(wall);

                if (newLeft + Cat.Width > wallLeft &&
                    newLeft < wallLeft + wall.Width &&
                    newTop + Cat.Height > wallTop &&
                    newTop < wallTop + wall.Height)
                {
                    return true;
                }
            }
            return false;
        }
        public Rect Bounds
        {
            get
            {
                double left = Canvas.GetLeft(Cat);
                double top = Canvas.GetTop(Cat);
                return new Rect(left, top, Cat.Width, Cat.Height);
            }
        }
        private double Distance(double x1, double y1, double x2, double y2)
        {
            return (new Vector(x2 - x1, y2 - y1)).Length;
        }
    }
}
