using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoundboardThreading
{
    /*
     * Plays and stops the audio.
     */
    public class AudioManager
    {
        public string CurrentlyPlaying { get; private set; }

        private readonly StorageFolder _storageFolder;
        private readonly MediaElement _playMusic;

        public AudioManager()
        {
            _storageFolder = ApplicationData.Current.LocalFolder;
            _playMusic = new MediaElement {AudioCategory = AudioCategory.Media};
            CurrentlyPlaying = "";
        }

        /*
         * Play audio from file
         * @param fileName Name of the file
         */
        public async void Play(string fileName)
        {
            var sound = await _storageFolder.GetFileAsync(fileName);
            _playMusic.SetSource(await sound.OpenAsync(FileAccessMode.Read), sound.ContentType);
            CurrentlyPlaying = fileName;
            _playMusic.Play();
        }

        /*
         * Stops the audio
         */
        public void Play()
        {
            _playMusic.Play();
        }
        public void Stop()
        {
            _playMusic.Stop();
        }

        public void Pause()
        {
            _playMusic.Pause();
        }
    }
}
