using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SoundboardThreading
{
    public enum DownloadResult
    {
        Ok,
        Fail,
        Cancel,
    }
    public sealed partial class AddDialog
    {
        public DownloadResult Result { get; private set; }
        public Sound Sound { get; private set; }
        public string Message { get; private set; }

        private readonly YoutubeDownloader _youtubeDownloader;
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
    }
}
