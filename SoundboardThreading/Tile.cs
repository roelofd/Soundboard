using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    class Tile
    {
        public TextBox textBox { get; set; }
        public TextBlock textBlock { get; set; }
        public Button playButton { get; set; }
        public Button downloadButton { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public Tile(TextBox textBox, TextBlock textBlock, Button playButton, Button downloadButton, int column, int row)
        {
            textBox = this.textBox;
            textBlock = this.textBlock;
            playButton = this.playButton;
            downloadButton = this.downloadButton;

            playButton.Click += Play_Button;
            downloadButton.Click += Download_Click;

            Column = column;
            Row = row;
        }

        string fileLocation;

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            var url = new Uri(textBox.Text);
            var downloader = new YoutubeDownloader();
            fileLocation = downloader.Download(url.ToString());
            if (fileLocation != null)
            {
                downloadButton.Visibility = Visibility.Collapsed;
                textBox.Visibility = Visibility.Collapsed;
                playButton.Visibility = Visibility.Visible;
                textBlock.Text = fileLocation.Split(".")[0];
                textBlock.Visibility = Visibility.Visible;
            }
        }

        private void Play_Button(object sender, RoutedEventArgs e)
        {
            var audioManager = new AudioManager();
            audioManager.Play(fileLocation);
        }
    }
}
