using System;
using System.Threading.Tasks;
using VideoLibrary;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Foundation;

namespace SoundboardThreading
{
    // Class that handles the downloading of youtube videos
    public class YoutubeDownloader
    {
        // The folder where the downloaded files are stored
        private readonly StorageFolder _storageFolder;

        private readonly YouTube _youTube = YouTube.Default;

        private YouTubeVideo _video;
        private string _mp3FileName;

        public YoutubeDownloader()
        {
            // Set download folder to LocalState folder
            _storageFolder = ApplicationData.Current.LocalFolder;
        }

        /*
         * Write File
         * @return the name of the mp3 file
         */
        public Sound Download(string url)
        {
            _video = _youTube.GetVideo(url);

            WriteFileAsync(_video);

            _mp3FileName = _video.FullName.Substring(0, _video.FullName.Length - 14);
            var sound = new Sound(_mp3FileName, _mp3FileName + ".mp3");

            sound.VideoName = _video.FullName;

            return sound;
        }

        /*
         * Method for writing the video and audio files
         */
        private async void WriteFileAsync(YouTubeVideo video)
        {
            StorageFile mp4StorageFile = await _storageFolder.CreateFileAsync(video.FullName, CreationCollisionOption.ReplaceExisting); // Store the video as a MP4
            await FileIO.WriteBytesAsync(mp4StorageFile, video.GetBytes());

            _mp3FileName = mp4StorageFile.Name.Substring(0, mp4StorageFile.Name.Length - 14);
            StorageFile mp3StorageFile = await _storageFolder.CreateFileAsync(_mp3FileName + ".mp3", CreationCollisionOption.ReplaceExisting);
            var profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
            await ToAudioAsync(mp4StorageFile, mp3StorageFile, profile);
        }

        /*
         * Method for converting the MP4 to MP3
         * Writes to the destination file
         */
        private async Task ToAudioAsync(StorageFile source, StorageFile destination, MediaEncodingProfile profile)
        {
            var transcoder = new MediaTranscoder(); 
            var prepareOp = await transcoder.PrepareFileTranscodeAsync(source, destination, profile);   // Prepare the file for conversion

            if (prepareOp.CanTranscode)
            {
                var transcodeOp = prepareOp.TranscodeAsync();   // Conversion

                // Progress handler
                transcodeOp.Progress += TranscodeProgress;
                // Completion handler
                transcodeOp.Completed += TranscodeComplete;
            }
            else
            {
                switch (prepareOp.FailureReason)    
                {
                    case TranscodeFailureReason.CodecNotFound:
                        System.Diagnostics.Debug.WriteLine("Codec not found.");
                        break;
                    case TranscodeFailureReason.InvalidProfile:
                        System.Diagnostics.Debug.WriteLine("Invalid profile.");
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown failure.");
                        break;
                }
            }
        }

        /*
         * Method for displaying the progress
         */
        private void TranscodeProgress(IAsyncActionWithProgress<double> asyncInfo, double percent)
        {
            // Display or handle progress info.
            System.Diagnostics.Debug.WriteLine(percent);
        }

        /*
         *  Method for displaying the completion
         */
        private void TranscodeComplete(IAsyncActionWithProgress<double> asyncInfo, AsyncStatus status)
        {
            asyncInfo.GetResults();
            if (asyncInfo.Status == AsyncStatus.Completed)
            {
                // Display or handle complete info.
                System.Diagnostics.Debug.WriteLine("Conversion success!");
            }
            else if (asyncInfo.Status == AsyncStatus.Canceled)
            {
                // Display or handle cancel info.
                System.Diagnostics.Debug.WriteLine("Conversion canceled!");
            }
            else
            {
                // Display or handle error info.
                System.Diagnostics.Debug.WriteLine("Conversion failed!");
            }
        }
    }
}
