using System;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace SoundboardThreading
{
    class AudioManager
    {
        public async void Play(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var sound = await storageFolder.GetFileAsync(fileName);
            var playMusic = new Windows.UI.Xaml.Controls.MediaElement();
            playMusic.AudioCategory = AudioCategory.Media;
            playMusic.SetSource(await sound.OpenAsync(FileAccessMode.Read), sound.ContentType);
            playMusic.Play();
        }
    }
}
