using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;

namespace SoundboardThreading
{
    public class Sound
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string VideoName { get; set; }

        public Sound(string title, string location)
        {
            Title = title;
            FileName = location;
        }
    }

    public class SoundManager
    {
        public static ObservableCollection<Sound> GetSounds()
        {
            var sounds = new ObservableCollection<Sound>
            {
                new Sound("Title", "SomeLocation"),
            };

            return sounds;
        }
    }
}
