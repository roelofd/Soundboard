using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoundboardThreading
{
    /*
     * Plays and stops the audio.
     */
    class AudioManager
    {
        private readonly StorageFolder _storageFolder;
        private readonly MediaElement _playMusic;

        public AudioManager()
        {
            _storageFolder = ApplicationData.Current.LocalFolder;
            _playMusic = new MediaElement {AudioCategory = AudioCategory.Media};
        }

        /*
         * Play audio from file
         * @param fileName Name of the file
         */
        public async void Play(string fileName)
        {
            var sound = await _storageFolder.GetFileAsync(fileName);
            _playMusic.SetSource(await sound.OpenAsync(FileAccessMode.Read), sound.ContentType);
            _playMusic.Play();
        }

        /*
         * Stops the audio
         */
        public void Stop()
        {
            _playMusic.Stop();
        }
    }
}
