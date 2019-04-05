using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    class Tile
    {
        // Instatiate tile UI items
        public TextBox textBox { get; set; }
        public TextBlock textBlock { get; set; }
        public Button playButton { get; set; }
        public Button downloadButton { get; set; }
        public ProgressBar progressBar { get; set; }

        string fileLocation;    // File location of the corresponding sound

        public Tile(TextBox _textBox, TextBlock _textBlock, Button _playButton, Button _downloadButton, ProgressBar _progressBar)
        {
            textBox = _textBox;
            textBlock = _textBlock;
            playButton = _playButton;
            downloadButton = _downloadButton;
            progressBar = _progressBar;

            downloadButton.Click += Button_Click;
            playButton.Click += Play_Button;
        }

        /*
         * Click event handler for clicking the donwload button on a tile
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = new Uri(textBox.Text);                    // Save url
            var downloader = new YoutubeDownloader();           // Create a downloader
            fileLocation = downloader.Download(url.ToString()); // Download to certain file location
            //updateProgress(downloader);
            if (fileLocation != null)
            {
                downloadButton.Visibility = Visibility.Collapsed;
                textBox.Visibility = Visibility.Collapsed;
                playButton.Visibility = Visibility.Visible;
                textBlock.Text = fileLocation.Split(".")[0];
                textBlock.Visibility = Visibility.Visible;
            }
        }

        /*
         * Click event handler for clicking the play button
         */
        private void Play_Button(object sender, RoutedEventArgs e)
        {
            var audioManager = new AudioManager();
            audioManager.Play(fileLocation);
        }

        /*
         * Method for updating the progress of the download
         */
        private async Task updateProgress(YoutubeDownloader youtubeDownloader)
        {
            double progress = 0;
            while(progress < 100)
            {
                progress = youtubeDownloader.getProgress();
                progressBar.Value = progress;
            }
        }
    }
}
