using System;
using System.Threading.Tasks;
using VideoLibrary;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Foundation;
using Windows.UI.Popups;

namespace SoundboardThreading
{
    // Class that handles the downloading of youtube videos
    class YoutubeDownloader
    {

        private StorageFolder _storageFolder;       // The folder where the downloaded files are stored
        private YouTube _youTube;                   // Class used for getting videos
        private double progressPercentage = 0;      // Percentage of the download progress

        public YoutubeDownloader()
        {
            _storageFolder = ApplicationData.Current.LocalFolder;   // Set download folder
            _youTube = YouTube.Default;                             // Set Youtube class
        }



        /*
         * Method called for downloading the actual video  
         */
        public string Download(string url)
        {
            YouTubeVideo video = _youTube.GetVideo(url);    // Get video by URL

            // Check if video is encrypted
            if (video.IsEncrypted)
            {
                var message = new MessageDialog($"{video.FullName} is encrypted!");
                message.ShowAsync();
                return null;
            }

            WriteFileAsync(video);      
            return video.FullName + ".mp3";     // Return the downloaded MP3
        }


        /*
         * Method for getting the current progress
         */
        public double getProgress()
        {
            return progressPercentage;
        }

        /*
         * Methed for writing the video 
         */
        private async void WriteFileAsync(YouTubeVideo video)
        {
            StorageFile mp4StorageFile = await _storageFolder.CreateFileAsync(video.FullName, CreationCollisionOption.ReplaceExisting); // Store the video as a MP4
            await FileIO.WriteBytesAsync(mp4StorageFile, video.GetBytes());     
            
            StorageFile mp3StorageFile = await _storageFolder.CreateFileAsync(mp4StorageFile.Name + ".mp3", CreationCollisionOption.ReplaceExisting);
            var profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
            await ToAudioAsync(mp4StorageFile, mp3StorageFile, profile);     // Convert the MP4 to MP3
        }

        /*
         * Method for converting the MP4 to MP3
         */
        private async Task ToAudioAsync(StorageFile source, StorageFile destination, MediaEncodingProfile profile)
        {
            var transcoder = new MediaTranscoder(); 
            var prepareOp = await transcoder.PrepareFileTranscodeAsync(source, destination, profile);   // Prepare the file for conversion

            if (prepareOp.CanTranscode)
            {
                var transcodeOp = prepareOp.TranscodeAsync();   // Conversion

                
                transcodeOp.Progress +=                                         // Save progress
                    new AsyncActionProgressHandler<double>(TranscodeProgress);
                transcodeOp.Completed +=                                        // Save completion
                    new AsyncActionWithProgressCompletedHandler<double>(TranscodeComplete);
            }
            else
            {   
                switch (prepareOp.FailureReason)    // If failed, print failure
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
            progressPercentage = percent;
            System.Diagnostics.Debug.WriteLine(percent);
        }

        /*
         *  Method for displaying the completion
         */
        private async void TranscodeComplete(IAsyncActionWithProgress<double> asyncInfo, AsyncStatus status)
        {
            asyncInfo.GetResults();
            if (asyncInfo.Status == AsyncStatus.Completed)
            {
                // Display or handle complete info.
                progressPercentage = 100;
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
