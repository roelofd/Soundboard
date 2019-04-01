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
            var a = vid.Uri;

            if (vid.IsEncrypted) //Als een video encrypted is kan je het niet opslaan
            {
                return null;
            }

            var newFile = await storageFolder.CreateFileAsync(vid.FullName, CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteBytesAsync(newFile, vid.GetBytes());

            if (Equals(extension, AudioFormat.Mp3))
            {
                var destination = await storageFolder.CreateFileAsync(newFile.Name + ".mp3");
                var profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
                await ToAudioAsync(newFile, destination, profile);
            }
            else if (Equals(extension, AudioFormat.Opus))
            {
                var destination = await storageFolder.CreateFileAsync(newFile.Name + ".opus");
                var profile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.High);//nog niet getest
                await ToAudioAsync(newFile, destination, profile);
            }
            else if (Equals(extension, AudioFormat.Aac))
            {
                var destination = await storageFolder.CreateFileAsync(newFile.Name + ".aac");
                var profile = MediaEncodingProfile.CreateM4a(AudioEncodingQuality.High);//kan ook mp3
                await ToAudioAsync(newFile, destination, profile);
            }
            else if (Equals(extension, AudioFormat.Vorbis))
            {
                var destination = await storageFolder.CreateFileAsync(newFile.Name + ".ogg");
                var profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);//maakt niet uit wat je hier doet want hij doet het niet
                await ToAudioAsync(newFile, destination, profile);
            }
            else if (Equals(extension, AudioFormat.Unknown))
            {
                var destination = await storageFolder.CreateFileAsync(newFile.Name + ".mp3");
                var profile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
                await ToAudioAsync(newFile, destination, profile);
            }
            return null;
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
