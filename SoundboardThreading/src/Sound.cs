using Windows.UI;
using Windows.UI.Xaml.Media;

namespace SoundboardThreading
{
    public class Sound
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string VideoName { get; set; }
        public SolidColorBrush Color { get; set; }
        public SolidColorBrush ForeGround { get; set; }

        public Sound(string title, string location)
        {
            Title = title;
            FileName = location;
            Color = new SolidColorBrush(Colors.LightGray);
            ForeGround = new SolidColorBrush(Colors.Black);
        }
    }
}
