using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    class Tile
    {
        public TextBox TextBox { get; set; }
        public TextBlock TextBlock { get; set; }
        public Button PlayButton { get; set; }
        public Button DownloadButton { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public Tile(TextBox textBox, TextBlock textBlock, Button playButton, Button downloadButton, int column, int row)
        {
            createTextBox(textBox, column, row);
            createTextBlock(textBlock, column, row);
            createPlayButton(playButton, column, row);
            createDownloadButton(downloadButton, column, row);
            
            Column = column;
            Row = row;
        }

        private void createTextBox(TextBox textBox, int column, int row)
        {
            textBox.Name = "textBox" + column + row;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Width = 350;
            textBox.PlaceholderText = "Paste Url here";
            textBox.Visibility = Visibility.Visible;
            Grid.SetColumn(textBox, column);
            Grid.SetRow(textBox, row);
            TextBox = textBox;
        }

        private void createTextBlock(TextBlock textBlock, int column, int row)
        {
            textBlock.Name = "textBlock" + column + row;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Visibility = Visibility.Collapsed;
            Grid.SetColumn(textBlock, column);
            Grid.SetRow(textBlock, row);
            TextBlock = textBlock;
        }

        private void createPlayButton(Button playButton, int column, int row)
        {
            playButton.Name = "playButton" + column + row;
            playButton.Content = "Play";
            playButton.HorizontalAlignment = HorizontalAlignment.Center;
            playButton.VerticalAlignment = VerticalAlignment.Top;
            playButton.Visibility = Visibility.Collapsed;
            Grid.SetColumn(playButton, column);
            Grid.SetRow(playButton, row);
            playButton.Click += Play_Button;
            PlayButton = playButton;
        }

        private void createDownloadButton(Button downloadButton, int column, int row)
        {
            downloadButton.Name = "downloadButton" + column + row;
            downloadButton.Content = "Download";
            downloadButton.HorizontalAlignment = HorizontalAlignment.Center;
            downloadButton.VerticalAlignment = VerticalAlignment.Bottom;
            downloadButton.Visibility = Visibility.Visible;
            Grid.SetColumn(downloadButton, column);
            Grid.SetRow(downloadButton, row);
            downloadButton.Click += Button_Click;
            DownloadButton = downloadButton;
        }

        string fileLocation;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = new Uri(TextBox.Text);
            var downloader = new YoutubeDownloader();
            fileLocation = downloader.Download(url.ToString());
            if (fileLocation != null)
            {
                DownloadButton.Visibility = Visibility.Collapsed;
                TextBox.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                TextBlock.Text = fileLocation.Split(".")[0];
                TextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Play_Button(object sender, RoutedEventArgs e)
        {
            var audioManager = new AudioManager();
            audioManager.Play(fileLocation);
        }
    }
}
