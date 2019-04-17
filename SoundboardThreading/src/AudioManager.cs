using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    /*
     * Plays and stops the audio.
     */
    public class AudioManager
    {
        public string CurrentlyPlaying { get; private set; }
        public MediaElement PlayMusic { get; }

        private readonly StorageFolder _storageFolder;

        public AudioManager(MediaElement mediaElement)
        {
            _storageFolder = ApplicationData.Current.LocalFolder;
            PlayMusic = mediaElement;
            CurrentlyPlaying = "";
        }

        /*
         * Play audio from file
         * @param fileName Name of the file
         */
        public async void Play(string fileName)
        {
            var sound = await _storageFolder.GetFileAsync(fileName);
            PlayMusic.SetSource(await sound.OpenAsync(FileAccessMode.Read), sound.ContentType);
            CurrentlyPlaying = fileName;
            PlayMusic.Play();
        }

        /*
         * Stops the audio
         */
        public void Play()
        {
            PlayMusic.Play();
        }
        public void Stop()
        {
            PlayMusic.Stop();
        }

        public void Pause()
        {
            PlayMusic.Pause();
        }
    }
}
