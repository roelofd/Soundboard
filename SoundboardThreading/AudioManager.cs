using System;
using System.Threading;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoundboardThreading
{
    class AudioManager
    {
        private StorageFolder storageFolder;
        private MediaElement playMusic;

        public AudioManager()
        {
            storageFolder = ApplicationData.Current.LocalFolder;
            playMusic = new Windows.UI.Xaml.Controls.MediaElement {AudioCategory = AudioCategory.Media};
        }

        public async void Play(string fileName)
        {
            var sound = await storageFolder.GetFileAsync(fileName);
            playMusic.SetSource(await sound.OpenAsync(FileAccessMode.Read), sound.ContentType);
            playMusic.Play();
        }

        public void Stop()
        {
            playMusic.Stop();
        }
    }
}
