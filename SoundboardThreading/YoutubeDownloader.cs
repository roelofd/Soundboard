using System;
using System.Threading.Tasks;
using VideoLibrary;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Foundation;

namespace SoundboardThreading
{
    class YoutubeDownloader
    {

        private StorageFolder _storageFolder;
        private YouTube _youTube;

        public YoutubeDownloader()
        {
            _storageFolder = ApplicationData.Current.LocalFolder;
            _youTube = YouTube.Default;
        }

        public string Download(string url)
        {
            YouTubeVideo video = _youTube.GetVideo(url);
            WriteFileAsync(video);
            return "";
        }

        private async void WriteFileAsync(YouTubeVideo video)
        {
            StorageFile newMp4 = await _storageFolder.CreateFileAsync(video.FullName, CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteBytesAsync(newMp4, video.GetBytes());

             
//            StorageFile newMp3 = await _storageFolder.CreateFileAsync(newMp4.Name + ".mp3");
//            var profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
//            await ToAudioAsync(newMp4, newMp3, profile);

        }

        private async Task<string> ToAudioAsync(StorageFile source, StorageFile destination, MediaEncodingProfile profile)
        {
            var transcoder = new MediaTranscoder();
            var prepareOp = await transcoder.PrepareFileTranscodeAsync(source, destination, profile);

            if (prepareOp.CanTranscode)
            {
                var transcodeOp = prepareOp.TranscodeAsync();

                transcodeOp.Progress +=
                    new AsyncActionProgressHandler<double>(TranscodeProgress);
                transcodeOp.Completed +=
                    new AsyncActionWithProgressCompletedHandler<double>(TranscodeComplete);
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
            return null;
        }

        private void TranscodeProgress(IAsyncActionWithProgress<double> asyncInfo, double percent)
        {
            // Display or handle progress info.
        }

        private void TranscodeComplete(IAsyncActionWithProgress<double> asyncInfo, AsyncStatus status)
        {
            asyncInfo.GetResults();
            if (asyncInfo.Status == AsyncStatus.Completed)
            {
                // Display or handle complete info.
            }
            else if (asyncInfo.Status == AsyncStatus.Canceled)
            {
                // Display or handle cancel info.
            }
            else
            {
                // Display or handle error info.
            }
        }
    }
}
