using System;
using System.Threading.Tasks;
using System.IO;
using VideoLibrary;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Media.Transcoding;
using Windows.Foundation;

namespace SoundboardThreading
{
    class YoutubeDownloader
    {

        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        public YoutubeDownloader(string url)
        {
            DownloadAsync(url);
        }

        public async Task<string> DownloadAsync(string url)
        {
            var video = new Uri(url);
            var source = storageFolder; 
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo(video.ToString());
            var extension = vid.AudioFormat;

            if (vid.IsEncrypted) //Als een video encrypted is kan je het niet opslaan
            {
                return null;
            }

            StorageFile newFile = await storageFolder.CreateFileAsync(vid.FullName, CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteBytesAsync(newFile, vid.GetBytes());

            if (Equals(extension, AudioFormat.Mp3))
            {
                StorageFile destination = await storageFolder.CreateFileAsync(newFile.Name + ".mp3");
                MediaEncodingProfile profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
                await ToAudioAsync(newFile, destination, profile);
                return null;
            }
            if (Equals(extension, AudioFormat.Opus))
            {
                StorageFile destination = await storageFolder.CreateFileAsync(newFile.Name + ".opus");
                MediaEncodingProfile profile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.High);//nog niet getest
                await ToAudioAsync(newFile, destination, profile);
                return null;
            }
            if (Equals(extension, AudioFormat.Aac))
            {
                StorageFile destination = await storageFolder.CreateFileAsync(newFile.Name + ".aac");
                MediaEncodingProfile profile = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.High);//kan ook mp3
                await ToAudioAsync(newFile, destination, profile);
                return null;
            }
            if (Equals(extension, AudioFormat.Vorbis))
            {
                StorageFile destination = await storageFolder.CreateFileAsync(newFile.Name + ".ogg");
                MediaEncodingProfile profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);//maakt niet uit wat je hier doet want hij doet het niet
                await ToAudioAsync(newFile, destination, profile);
                return null;
            }
            if (Equals(extension, AudioFormat.Unknown))
            {
                return null;
            }
            return null;
        }

        private async Task<string> ToAudioAsync(StorageFile source, StorageFile destination, MediaEncodingProfile profile)
        {
            MediaTranscoder transcoder = new MediaTranscoder();
            PrepareTranscodeResult prepareOp = await transcoder.PrepareFileTranscodeAsync(source, destination, profile);

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
