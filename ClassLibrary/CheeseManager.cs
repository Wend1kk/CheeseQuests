using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameLibrary
{
    public class CheeseManager
    {
        private List<Image> cheeses = new List<Image>();
        private TextBlock cheeseCountText;
        private Image DoorImage;
        public int CollectedCheeseCount { get; private set; } = 0;
        public int RequiredCheese { get; private set; }
        public CheeseManager(TextBlock cheeseText, Image door, int requiredCheese)
        {
            cheeseCountText = cheeseText;
            DoorImage = door;
            RequiredCheese = requiredCheese;
        }
        public void AddCheeseFromCanvas(Canvas canvas, int cheeseCount)
        {
            for (int i = 1; i <= cheeseCount; i++)
            {
                Image cheese = (Image)canvas.FindName($"Cheese{i}");
                if (cheese != null)
                {
                    cheeses.Add(cheese);
                }
            }
        }
        public void CheckCheeseCollection(Image MouseImage)
        {
            double mouseLeft = Canvas.GetLeft(MouseImage);
            double mouseTop = Canvas.GetTop(MouseImage);
            List<Image> collectedCheese = new List<Image>(); 
            foreach (Image cheese in cheeses)
            {
                double cheeseLeft = Canvas.GetLeft(cheese);
                double cheeseTop = Canvas.GetTop(cheese);
                if (mouseLeft + MouseImage.Width > cheeseLeft &&
                    mouseLeft < cheeseLeft + cheese.Width &&
                    mouseTop + MouseImage.Height > cheeseTop &&
                    mouseTop < cheeseTop + cheese.Height)
                {
                    cheese.Visibility = Visibility.Hidden;
                    collectedCheese.Add(cheese); 
                }
            }
            if (collectedCheese.Count > 0) 
            {
                foreach (Image cheese in collectedCheese)
                {
                    cheeses.Remove(cheese); 
                }
                CollectedCheeseCount += collectedCheese.Count;
                cheeseCountText.Text = CollectedCheeseCount.ToString("D4");
                CheckDoorUnlock();
            }
        }
        public void CheckDoorUnlock()
        {
            if (CollectedCheeseCount >= RequiredCheese)
            {
                DoorImage.Source = new BitmapImage(new Uri("Images/OpenDoor.png", UriKind.Relative));
            }
        }
    }
}
