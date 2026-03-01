using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cheese_Quest;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cheese_Quest
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(MediaPlayer mediaPlayer)
        {
            InitializeComponent();
            this.mediaPlayer = mediaPlayer;
        }
        private MediaPlayer mediaPlayer;
        private bool SoundOn = true;
        private void SoundsButton_Click(object sender, RoutedEventArgs e)
        {
            if(SoundOn)
            {
                mediaPlayer.Pause();
                SoundImage.Source = new BitmapImage(new Uri("Images/SoundOff.png", UriKind.Relative));
                SoundOn = false;
                
            }
            else
            {
                mediaPlayer.Play();
                SoundImage.Source = new BitmapImage(new Uri("Images/SoundOn.png", UriKind.Relative));
                SoundOn = true;
            }
        }
    }
}
