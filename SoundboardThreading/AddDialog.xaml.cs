using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SoundboardThreading
{
    public enum DownloadResult
    {
        Ok,
        Fail,
        Cancel,
        Nothing
    }
    public sealed partial class AddDialog : ContentDialog
    {
        public DownloadResult Result { get; private set; }
        public Sound Sound { get; private set; }
        public string Message { get; private set; }

        private YoutubeDownloader _youtubeDownloader;
        public AddDialog(YoutubeDownloader youtubeDownloader)
        {
            InitializeComponent();
            _youtubeDownloader = youtubeDownloader;
        }

        private void ContentDialog_DownloadButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                Sound = _youtubeDownloader.Download(new Uri(AddTextBox.Text).ToString());
                //DeleteFile();
                Message = "Download successful!";
                Result = DownloadResult.Ok;
            }
            catch
            {
                Message = "Download failed! Did you use a correct youtube link?";
                Result = DownloadResult.Fail;
            }
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Message = "Download cancelled!";
            Result = DownloadResult.Cancel;
        }

        private async void DeleteFile()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var mp4 = await storageFolder.GetFileAsync(Sound.VideoName);
            await mp4.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
